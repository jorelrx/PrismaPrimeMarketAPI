using MediatR;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.ConfirmPasswordReset;

/// <summary>
/// Command para confirmar reset de senha com token
/// </summary>
public record ConfirmPasswordResetCommand(
    string Token,
    string NewPassword
) : IRequest<Unit>;
