using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.Auth;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;

/// <summary>
/// Handler para autenticar usuário e gerar tokens JWT
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginCommandHandler(
        UserManager<User> userManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Busca o usuário pelo email
        var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new InvalidCredentialsException();

        // Verifica a senha
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            throw new InvalidCredentialsException();

        // Verifica se o usuário está ativo
        if (!user.IsActive)
            throw new InvalidCredentialsException("Usuário inativo");

        // Registra o login
        user.RegisterLogin();
        await _unitOfWork.CommitAsync(cancellationToken);

        // Obtém os roles do usuário
        var roles = await _userManager.GetRolesAsync(user);

        // Gera os tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        // Calcula as datas de expiração
        var accessExpiration = _jwtTokenService.ParseExpirationConfig("Jwt:AccessExpiration");
        var refreshExpiration = _jwtTokenService.ParseExpirationConfig("Jwt:RefreshExpiration");

        var accessExpiresAt = DateTime.UtcNow.Add(accessExpiration);
        var refreshExpiresAt = DateTime.UtcNow.Add(refreshExpiration);

        // Salva o refresh token no banco
        var refreshTokenEntity = Domain.Entities.RefreshToken.Create(refreshToken, user.Id, refreshExpiresAt);
        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // Mapeia para DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = [.. roles];

        return new AuthResponseDto
        {
            User = userDto,
            Tokens = new AuthTokensDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiresAt = accessExpiresAt,
                RefreshTokenExpiresAt = refreshExpiresAt
            }
        };
    }
}
