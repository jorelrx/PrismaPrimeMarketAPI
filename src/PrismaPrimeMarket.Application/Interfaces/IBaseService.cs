using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.Interfaces;

public interface IBaseService<TEntity, TDto> 
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, TDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
