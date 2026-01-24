using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Domain.Interfaces.Repositories;

/// <summary>
/// Interface do repositório para a entidade User
/// </summary>
public interface IUserRepository : IBaseRepository<User>
{
    /// <summary>
    /// Obtém um usuário pelo e-mail
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo nome de usuário
    /// </summary>
    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um usuário pelo CPF
    /// </summary>
    Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe um usuário com o e-mail informado
    /// </summary>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe um usuário com o nome de usuário informado
    /// </summary>
    Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se existe um usuário com o CPF informado
    /// </summary>
    Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém usuários ativos (não excluídos)
    /// </summary>
    Task<List<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém usuários por role
    /// </summary>
    Task<List<User>> GetUsersByRoleAsync(string roleName, CancellationToken cancellationToken = default);
}
