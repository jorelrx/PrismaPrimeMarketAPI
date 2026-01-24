using MediatR;
using PrismaPrimeMarket.Application.DTOs.User;

namespace PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUsers;

/// <summary>
/// Query para obter lista de usuários ativos
/// </summary>
public record GetUsersQuery() : IRequest<List<UserDto>>;
