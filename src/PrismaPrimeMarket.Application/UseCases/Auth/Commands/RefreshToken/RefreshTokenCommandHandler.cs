using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.Auth;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RefreshToken;

/// <summary>
/// Handler para renovar access token usando refresh token
/// </summary>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthTokensDto>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler(
        UserManager<User> userManager,
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthTokensDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // Busca o refresh token no banco
        var refreshTokenEntity = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken, cancellationToken);
        if (refreshTokenEntity == null)
            throw new InvalidTokenException("Refresh token inválido");

        // Verifica se o token está ativo
        if (!refreshTokenEntity.IsActive)
            throw new InvalidTokenException("Refresh token inválido ou expirado");

        // Busca o usuário
        var user = await _userManager.FindByIdAsync(refreshTokenEntity.UserId.ToString());
        if (user == null || !user.IsActive)
            throw new InvalidTokenException("Token inválido ou expirado");

        // Obtém os roles do usuário
        var roles = await _userManager.GetRolesAsync(user);

        // Gera novos tokens
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email!, roles);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

        // Calcula as datas de expiração
        var accessExpiration = _jwtTokenService.ParseExpirationConfig("Jwt:AccessExpiration");
        var refreshExpiration = _jwtTokenService.ParseExpirationConfig("Jwt:RefreshExpiration");

        var accessExpiresAt = DateTime.UtcNow.Add(accessExpiration);
        var refreshExpiresAt = DateTime.UtcNow.Add(refreshExpiration);

        // Revoga o token antigo
        refreshTokenEntity.Revoke(newRefreshToken);

        // Salva o novo refresh token
        var newRefreshTokenEntity = Domain.Entities.RefreshToken.Create(newRefreshToken, user.Id, refreshExpiresAt);
        await _refreshTokenRepository.AddAsync(newRefreshTokenEntity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new AuthTokensDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = accessExpiresAt,
            RefreshTokenExpiresAt = refreshExpiresAt
        };
    }
}
