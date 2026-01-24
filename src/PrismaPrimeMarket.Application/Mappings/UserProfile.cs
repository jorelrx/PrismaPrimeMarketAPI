using AutoMapper;
using PrismaPrimeMarket.Application.DTOs.User;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.RegisterUser;
using PrismaPrimeMarket.Application.UseCases.Users.Commands.UpdateUserProfile;
using PrismaPrimeMarket.Domain.Entities;

namespace PrismaPrimeMarket.Application.Mappings;

/// <summary>
/// Profile do AutoMapper para mapeamento de User
/// </summary>
public class UserProfile : Profile
{
    public UserProfile()
    {
        // Entity → DTO
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.CPF != null ? src.CPF.GetFormatted() : null))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone != null ? src.Phone.GetFormatted() : null))
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Roles serão definidas manualmente no handler

        // DTOs → Commands
        CreateMap<CreateUserDto, RegisterUserCommand>();
        CreateMap<UpdateUserProfileDto, UpdateUserProfileCommand>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore()); // UserId virá da rota
    }
}
