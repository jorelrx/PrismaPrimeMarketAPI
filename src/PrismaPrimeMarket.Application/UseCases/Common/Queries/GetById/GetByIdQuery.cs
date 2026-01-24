using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.UseCases.Common.Queries.GetById;

/// <summary>
/// Query genérica para buscar entidade por ID
/// </summary>
/// <typeparam name="TDto">Tipo do DTO de retorno</typeparam>
public record GetByIdQuery<TDto>(Guid Id) : IQuery<Response<TDto>>
    where TDto : class;
