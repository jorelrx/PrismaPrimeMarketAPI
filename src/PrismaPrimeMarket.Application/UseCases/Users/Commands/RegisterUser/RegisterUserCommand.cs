using MediatR;
using PrismaPrimeMarket.Application.DTOs.User;

namespace PrismaPrimeMarket.Application.UseCases.Users.Commands.RegisterUser;

/// <summary>
/// Command para registrar um novo usuário
/// </summary>
public record RegisterUserCommand(
    string UserName,
    string FirstName,
    string Password,
    string? Email = null,
    string? LastName = null,
    string? CPF = null,
    string? PhoneNumber = null,
    DateTime? BirthDate = null
) : IRequest<UserDto>;
