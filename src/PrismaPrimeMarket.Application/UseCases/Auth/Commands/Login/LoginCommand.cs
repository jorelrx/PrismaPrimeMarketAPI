using MediatR;
using PrismaPrimeMarket.Application.DTOs.Auth;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.Login;

/// <summary>
/// Command para autenticar usuário e gerar tokens JWT
/// </summary>
public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;
