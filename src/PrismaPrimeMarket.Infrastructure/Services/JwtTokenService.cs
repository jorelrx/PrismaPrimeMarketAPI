using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.Infrastructure.Services;

/// <summary>
/// Implementação do serviço de geração e validação de tokens JWT
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _accessSecret;
    private readonly string _accessExpiration;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _accessSecret = _configuration["Jwt:AccessSecret"] ?? throw new InvalidOperationException("JWT AccessSecret não configurado");
        _accessExpiration = _configuration["Jwt:AccessExpiration"] ?? "15m";
        _issuer = _configuration["Jwt:Issuer"] ?? "PrismaPrimeMarket";
        _audience = _configuration["Jwt:Audience"] ?? "PrismaPrimeMarket";
    }

    public string GenerateAccessToken(Guid userId, string email, IEnumerable<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Adiciona roles como claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expiration = ParseExpiration(_accessExpiration);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(expiration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_accessSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    public Guid? GetUserIdFromToken(string token)
    {
        var principal = ValidateToken(token);
        var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
        
        if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    /// <summary>
    /// Método público para parsear configurações de tempo de expiração
    /// </summary>
    public TimeSpan ParseExpirationConfig(string configKey)
    {
        var expiration = _configuration[configKey];
        return ParseExpiration(expiration ?? "15m");
    }

    private TimeSpan ParseExpiration(string expiration)
    {
        // Suporta formatos como "15m", "7d", "2h"
        if (string.IsNullOrEmpty(expiration))
            return TimeSpan.FromMinutes(15);

        if (expiration.Length < 2)
            return TimeSpan.FromMinutes(15);

        if (!int.TryParse(expiration[..^1], out var value) || value <= 0)
            return TimeSpan.FromMinutes(15);

        var unit = expiration[^1];

        return unit switch
        {
            'm' => TimeSpan.FromMinutes(value),
            'h' => TimeSpan.FromHours(value),
            'd' => TimeSpan.FromDays(value),
            _ => TimeSpan.FromMinutes(15)
        };
    }
}
