using System.Security.Claims;

namespace PrismaPrimeMarket.Domain.Interfaces;

/// <summary>
/// Interface para serviço de geração e validação de tokens JWT
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Gera um access token JWT para o usuário
    /// </summary>
    string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles);

    /// <summary>
    /// Gera um refresh token
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Valida um token JWT e retorna os claims
    /// </summary>
    ClaimsPrincipal? ValidateToken(string token);

    /// <summary>
    /// Obtém o userId de um token
    /// </summary>
    Guid? GetUserIdFromToken(string token);
}
