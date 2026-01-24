using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.UseCases.Common.Queries.GetList;

/// <summary>
/// Query genérica para buscar lista paginada de entidades
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
public record GetListQuery<TDto>(PaginationFilter Filter) : IQuery<PagedResponse<IEnumerable<TDto>>>
    where TDto : class;
