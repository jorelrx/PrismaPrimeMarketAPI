namespace PrismaPrimeMarket.Application.Common.Messaging;

/// <summary>
/// Interface base para Queries (CQRS)
/// Queries apenas leem dados, não modificam estado
/// </summary>
/// <typeparam name="TResponse">Tipo de resposta da query</typeparam>
public interface IQuery<out TResponse> : MediatR.IRequest<TResponse>
{
}
