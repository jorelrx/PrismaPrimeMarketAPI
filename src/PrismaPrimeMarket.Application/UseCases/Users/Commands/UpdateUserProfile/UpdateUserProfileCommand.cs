using MediatR;
using PrismaPrimeMarket.Application.DTOs.User;

namespace PrismaPrimeMarket.Application.UseCases.Users.Commands.UpdateUserProfile;

/// <summary>
/// Command para atualizar o perfil de um usuário
/// </summary>
public record UpdateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string? LastName = null,
    string? CPF = null,
    string? PhoneNumber = null,
    DateTime? BirthDate = null
) : IRequest<UserDto>;
