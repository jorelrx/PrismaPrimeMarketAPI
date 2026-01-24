using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUserById;

/// <summary>
/// Handler para obter um usuário por ID
/// </summary>
public class GetUserByIdQueryHandler(
    IUserRepository userRepository,
    UserManager<User> userManager,
    IMapper mapper) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        // Obtém roles do usuário
        var roles = await _userManager.GetRolesAsync(user);

        // Mapeia para DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();

        return userDto;
    }
}
