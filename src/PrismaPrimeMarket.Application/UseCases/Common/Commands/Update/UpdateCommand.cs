using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Update;

/// <summary>
/// Command genérico para atualizar entidade
/// </summary>
/// <typeparam name="TDto">Tipo do DTO</typeparam>
public record UpdateCommand<TDto>(Guid Id, TDto Data) : ICommand<Response<TDto>>
    where TDto : class;
