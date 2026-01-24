using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.Application.UseCases.Users.Commands.UpdateUserProfile;

/// <summary>
/// Handler para atualizar o perfil de um usuário
/// </summary>
public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserProfileCommandHandler(
        IUserRepository userRepository,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // Busca o usuário
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
            throw new UserNotFoundException(request.UserId);

        // Cria Value Objects
        CPF? cpf = null;
        if (!string.IsNullOrEmpty(request.CPF))
            cpf = CPF.Create(request.CPF);

        PhoneNumber? phone = null;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            phone = PhoneNumber.Create(request.PhoneNumber);

        // Atualiza o perfil usando o método do domínio
        user.UpdateProfile(
            request.FirstName,
            request.LastName,
            cpf,
            phone,
            request.BirthDate
        );

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        // Obtém roles do usuário
        var roles = await _userManager.GetRolesAsync(user);

        // Mapeia para DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();

        return userDto;
    }
}
