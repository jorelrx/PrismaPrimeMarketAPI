using AutoMapper;
using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.UseCases.Common.Queries.GetById;

/// <summary>
/// Handler genérico para buscar entidade por ID
/// </summary>
public class GetByIdQueryHandler<TEntity, TDto>(
    IBaseRepository<TEntity> repository,
    IMapper mapper)
    : IQueryHandler<GetByIdQuery<TDto>, Response<TDto>>
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<TDto>> Handle(GetByIdQuery<TDto> request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if (entity == null)
            return Response<TDto>.NotFound($"Recurso com ID {request.Id} não encontrado");

        var dto = _mapper.Map<TDto>(entity);
        return Response<TDto>.Retrieved(dto);
    }
}
