using AutoMapper;
using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Common;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Application.UseCases.Common.Queries.GetList;

/// <summary>
/// Handler genérico para buscar lista paginada de entidades
/// </summary>
public class GetListQueryHandler<TEntity, TDto>(
    IBaseRepository<TEntity> repository,
    IMapper mapper)
    : IQueryHandler<GetListQuery<TDto>, PagedResponse<IEnumerable<TDto>>>
    where TEntity : BaseEntity, IAggregateRoot
    where TDto : class
{
    private readonly IBaseRepository<TEntity> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<PagedResponse<IEnumerable<TDto>>> Handle(GetListQuery<TDto> request, CancellationToken cancellationToken)
    {
        var query = _repository.GetQuery();

        // Aplicar filtros personalizados seria feito aqui em handlers específicos
        // query = ApplyFilters(query, request.Filter);

        var totalCount = await _repository.CountAsync(query, cancellationToken);

        var filter = request.Filter;
        if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
        {
            query = query
                .Skip((filter.PageNumber.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value);
        }

        var items = await _repository.ExecuteQueryAsync(query, cancellationToken);
        var dtos = _mapper.Map<IEnumerable<TDto>>(items);

        var pageNumber = filter.PageNumber ?? 1;
        var pageSize = filter.PageSize ?? (totalCount > 0 ? totalCount : 10);

        return PagedResponse<IEnumerable<TDto>>.Create(dtos, pageNumber, pageSize, totalCount);
    }
}
