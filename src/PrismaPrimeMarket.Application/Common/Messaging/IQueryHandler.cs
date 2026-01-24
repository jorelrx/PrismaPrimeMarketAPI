namespace PrismaPrimeMarket.Application.Common.Messaging;

/// <summary>
/// Interface base para Query Handlers (CQRS)
/// </summary>
/// <typeparam name="TQuery">Tipo da query</typeparam>
/// <typeparam name="TResponse">Tipo de resposta</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : MediatR.IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
}
