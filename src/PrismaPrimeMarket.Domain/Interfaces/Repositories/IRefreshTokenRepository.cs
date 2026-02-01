using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para repositório de Refresh Tokens
/// </summary>
public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
{
    /// <summary>
    /// Obtém um refresh token pelo valor do token
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os refresh tokens ativos de um usuário
    /// </summary>
    Task<IEnumerable<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoga todos os refresh tokens de um usuário
    /// </summary>
    Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove tokens expirados do banco de dados
    /// </summary>
    Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default);
}
