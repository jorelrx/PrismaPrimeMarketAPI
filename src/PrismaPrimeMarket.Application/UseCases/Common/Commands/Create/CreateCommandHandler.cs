using AutoMapper;
using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Create;

/// <summary>
/// Handler genérico para criar entidade
/// </summary>
public class CreateCommandHandler<TEntity, TDto>(
    IBaseRepository<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : ICommandHandler<CreateCommand<TDto>, Response<TDto>>
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<TDto>> Handle(CreateCommand<TDto> request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TEntity>(request.Data);

        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(entity);
        return Response<TDto>.Created(dto);
    }
}
