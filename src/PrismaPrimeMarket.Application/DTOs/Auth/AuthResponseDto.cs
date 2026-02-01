using PrismaPrimeMarket.Application.DTOs.User;

namespace PrismaPrimeMarket.Application.DTOs.Auth;

/// <summary>
/// DTO com resposta completa de autenticação (usuário + tokens)
/// </summary>
public class AuthResponseDto
{
    public UserDto User { get; set; } = null!;
    public AuthTokensDto Tokens { get; set; } = null!;
}
