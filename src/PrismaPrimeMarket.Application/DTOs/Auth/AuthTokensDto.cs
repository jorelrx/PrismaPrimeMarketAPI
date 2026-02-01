namespace PrismaPrimeMarket.Application.DTOs.Auth;

/// <summary>
/// DTO contendo tokens de autenticação JWT
/// </summary>
public class AuthTokensDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}
