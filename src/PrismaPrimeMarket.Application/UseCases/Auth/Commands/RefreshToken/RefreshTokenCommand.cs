using MediatR;
using PrismaPrimeMarket.Application.DTOs.Auth;

namespace PrismaPrimeMarket.Application.UseCases.Auth.Commands.RefreshToken;

/// <summary>
/// Command para renovar access token usando refresh token
/// </summary>
public record RefreshTokenCommand(
    string RefreshToken
) : IRequest<AuthTokensDto>;
