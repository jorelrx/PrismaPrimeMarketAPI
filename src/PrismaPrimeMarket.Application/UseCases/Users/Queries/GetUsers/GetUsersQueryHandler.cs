using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;

namespace PrismaPrimeMarket.Application.UseCases.Users.Queries.GetUsers;

/// <summary>
/// Handler para obter lista de usuários
/// </summary>
public class GetUsersQueryHandler(
    IUserRepository userRepository,
    UserManager<User> userManager,
    IMapper mapper) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IMapper _mapper = mapper;

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetActiveUsersAsync(cancellationToken);

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = [.. roles];

            userDtos.Add(userDto);
        }

        return userDtos;
    }
}
