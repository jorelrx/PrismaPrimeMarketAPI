using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.Common.Messaging;

/// <summary>
/// Interface base para Commands (CQRS)
/// Commands modificam o estado da aplicação
/// </summary>
/// <typeparam name="TResponse">Tipo de resposta do command</typeparam>
public interface ICommand<out TResponse> : MediatR.IRequest<TResponse>
{
}

/// <summary>
/// Interface base para Commands sem resposta
/// </summary>
public interface ICommand : MediatR.IRequest
{
}
