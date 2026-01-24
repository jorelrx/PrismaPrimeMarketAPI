using AutoMapper;
using PrismaPrimeMarket.Application.Interfaces;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.Common.Services;

public abstract class BaseService<TEntity, TDto>(IBaseRepository<TEntity> repository, IMapper mapper, IUnitOfWork unitOfWork) : IBaseService<TEntity, TDto>
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    protected readonly IBaseRepository<TEntity> _repository = repository;
    protected readonly IMapper _mapper = mapper;
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    public virtual async Task<IEnumerable<TDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<TDto>>(entities);
    }

    public virtual async Task<TDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity == null ? null : _mapper.Map<TDto>(entity);
    }

    public virtual async Task<TDto> AddAsync(TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<TEntity>(dto);
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        return _mapper.Map<TDto>(entity);
    }

    public virtual async Task UpdateAsync(Guid id, TDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity != null)
        {
             _mapper.Map(dto, entity);
             await _repository.UpdateAsync(entity, cancellationToken);
             await _unitOfWork.CommitAsync(cancellationToken);
        }
    }

    public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
