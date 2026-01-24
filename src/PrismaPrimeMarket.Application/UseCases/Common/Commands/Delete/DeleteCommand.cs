using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Delete;

/// <summary>
/// Command genérico para excluir entidade
/// </summary>
public record DeleteCommand(Guid Id) : ICommand<Response<object>>;
