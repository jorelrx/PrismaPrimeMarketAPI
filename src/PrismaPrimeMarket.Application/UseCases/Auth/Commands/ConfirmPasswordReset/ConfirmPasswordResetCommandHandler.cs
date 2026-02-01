using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.ConfirmPasswordReset;

/// <summary>
/// Handler para confirmar reset de senha com token
/// </summary>
public class ConfirmPasswordResetCommandHandler : IRequestHandler<ConfirmPasswordResetCommand, Unit>
{
    private readonly UserManager<User> _userManager;
    private readonly IPasswordResetRepository _passwordResetRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmPasswordResetCommandHandler(
        UserManager<User> userManager,
        IPasswordResetRepository passwordResetRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _passwordResetRepository = passwordResetRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ConfirmPasswordResetCommand request, CancellationToken cancellationToken)
    {
        // Busca o token de reset no banco
        var passwordReset = await _passwordResetRepository.GetByTokenAsync(request.Token, cancellationToken);
        if (passwordReset == null || !passwordReset.IsValid)
            throw new InvalidTokenException("Token de reset de senha inválido ou expirado");

        // Busca o usuário
        var user = await _userManager.FindByIdAsync(passwordReset.UserId.ToString());
        if (user == null)
            throw new UserNotFoundException(passwordReset.UserId);

        // Reseta a senha usando o Identity
        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = result.Errors.ToDictionary(
                e => e.Code,
                e => new[] { e.Description }
            );
            throw new ValidationException(errors);
        }

        // Marca o token como usado
        passwordReset.MarkAsUsed();

        // Invalida todos os outros tokens de reset do usuário
        await _passwordResetRepository.InvalidateAllUserResetsAsync(user.Id, cancellationToken);

        // Revoga todos os refresh tokens do usuário (encerra sessões)
        await _refreshTokenRepository.RevokeAllUserTokensAsync(user.Id, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return Unit.Value;
    }
}
