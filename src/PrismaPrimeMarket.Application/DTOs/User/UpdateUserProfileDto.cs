namespace PrismaPrimeMarket.Application.DTOs.User;

/// <summary>
/// DTO para atualizar informações do perfil de usuário
/// </summary>
public class UpdateUserProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CPF { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? BirthDate { get; set; }
}
