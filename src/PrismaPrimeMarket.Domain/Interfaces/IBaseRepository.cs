using System.Linq.Expressions;
using PrismaPrimeMarket.Domain.Common;

namespace PrismaPrimeMarket.Domain.Interfaces;

public interface IBaseRepository<T> where T : class, IBaseEntity, IAggregateRoot
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    IQueryable<T> GetQuery();
    Task<List<T>> ExecuteQueryAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    Task<int> CountAsync(IQueryable<T> query, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
