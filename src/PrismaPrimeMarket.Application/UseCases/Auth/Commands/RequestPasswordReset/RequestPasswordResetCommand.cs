using MediatR;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RequestPasswordReset;

/// <summary>
/// Command para solicitar reset de senha
/// </summary>
public record RequestPasswordResetCommand(
    string Email
) : IRequest<Unit>;
