namespace PrismaPrimeMarket.Application.Common.Messaging;

/// <summary>
/// Interface base para Command Handlers (CQRS)
/// </summary>
/// <typeparam name="TCommand">Tipo do command</typeparam>
/// <typeparam name="TResponse">Tipo de resposta</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : MediatR.IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
}

/// <summary>
/// Interface base para Command Handlers sem resposta
/// </summary>
/// <typeparam name="TCommand">Tipo do command</typeparam>
public interface ICommandHandler<in TCommand> : MediatR.IRequestHandler<TCommand>
    where TCommand : ICommand
{
}
