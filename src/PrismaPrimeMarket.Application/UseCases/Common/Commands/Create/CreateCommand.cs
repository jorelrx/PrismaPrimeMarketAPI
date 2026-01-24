using PrismaPrimeMarket.Application.Common.Messaging;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.Application.UseCases.Common.Commands.Create;

/// <summary>
/// Command genérico para criar entidade
/// </summary>
/// <typeparam name="TDto">Tipo do DTO</typeparam>
public record CreateCommand<TDto>(TDto Data) : ICommand<Response<TDto>>
    where TDto : class;
