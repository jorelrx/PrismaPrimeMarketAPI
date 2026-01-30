using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para repositório de Password Resets
/// </summary>
public interface IPasswordResetRepository : IBaseRepository<PasswordReset>
{
    /// <summary>
    /// Obtém uma solicitação de reset de senha pelo token
    /// </summary>
    Task<PasswordReset?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém solicitações de reset válidas de um usuário
    /// </summary>
    Task<IEnumerable<PasswordReset>> GetValidResetsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Invalida todas as solicitações de reset de senha de um usuário
    /// </summary>
    Task InvalidateAllUserResetsAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove solicitações de reset expiradas do banco de dados
    /// </summary>
    Task RemoveExpiredResetsAsync(CancellationToken cancellationToken = default);
}
