using MediatR;
using PrismaPrimeMarket.Application.DTOs.User;

namespace PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUserById;

/// <summary>
/// Query para obter um usuário por ID
/// </summary>
public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
