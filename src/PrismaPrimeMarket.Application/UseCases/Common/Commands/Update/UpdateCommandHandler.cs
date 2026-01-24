using AutoMapper;
using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Update;

/// <summary>
/// Handler genérico para atualizar entidade
/// </summary>
public class UpdateCommandHandler<TEntity, TDto>(
    IBaseRepository<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : ICommandHandler<UpdateCommand<TDto>, Response<TDto>>
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<TDto>> Handle(UpdateCommand<TDto> request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return Response<TDto>.NotFound($"Recurso com ID {request.Id} não encontrado");

        _mapper.Map(request.Data, entity);

        await _repository.UpdateAsync(entity, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        var dto = _mapper.Map<TDto>(entity);
        return Response<TDto>.Updated(dto);
    }
}
