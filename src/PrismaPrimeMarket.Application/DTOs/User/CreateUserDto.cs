namespace PrismaPrimeMarket.Application.DTOs.User;

/// <summary>
/// DTO para criar um novo usuário
/// </summary>
public class CreateUserDto
{
    public string UserName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? CPF { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
}
