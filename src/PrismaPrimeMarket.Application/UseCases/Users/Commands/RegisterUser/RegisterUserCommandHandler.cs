using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Exceptions;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;
using PrismaPrimeMarket.Domain.ValueObjects;

namespace PrismaPrimeMarket.Application.UseCases.Users.Commands.RegisterUser;

/// <summary>
/// Handler para registrar um novo usuário
/// </summary>
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
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

    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Verifica se já existe usuário com o email (somente se fornecido)
        if (!string.IsNullOrEmpty(request.Email) && 
            await _userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new UserAlreadyExistsException(request.Email, "e-mail");

        // Verifica se já existe usuário com o username
        if (await _userRepository.ExistsByUserNameAsync(request.UserName, cancellationToken))
            throw new UserAlreadyExistsException(request.UserName, "nome de usuário");

        // Verifica CPF se foi fornecido
        if (!string.IsNullOrEmpty(request.CPF))
        {
            var cleanedCpf = System.Text.RegularExpressions.Regex.Replace(request.CPF, @"[^\d]", "");
            if (await _userRepository.ExistsByCpfAsync(cleanedCpf, cancellationToken))
                throw new UserAlreadyExistsException(request.CPF, "CPF");
        }

        // Cria os Value Objects
        CPF? cpf = null;
        if (!string.IsNullOrEmpty(request.CPF))
            cpf = CPF.Create(request.CPF);

        PhoneNumber? phone = null;
        if (!string.IsNullOrEmpty(request.PhoneNumber))
            phone = PhoneNumber.Create(request.PhoneNumber);

        // Cria o usuário usando o factory method do domínio
        var user = User.Create(
            request.UserName,
            request.FirstName,
            request.Email,
            request.LastName,
            cpf,
            phone,
            request.BirthDate
        );

        // Cria o usuário usando o Identity (para hash de senha)
        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new DomainException($"Erro ao criar usuário: {errors}");
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        // Mapeia para DTO
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = new List<string> { "Customer" };

        return userDto;
    }
}
