using System.Security.Claims;
using PrismaPrimeMarket.Domain.Interfaces;

namespace PrismaPrimeMarket.API.Middlewares;

/// <summary>
/// Middleware para autenticação JWT customizada
/// </summary>
public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtAuthenticationMiddleware> _logger;

    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IJwtTokenService jwtTokenService)
    {
        // Tenta extrair o token do header Authorization
        var token = ExtractTokenFromHeader(context);

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                // Valida o token
                var principal = jwtTokenService.ValidateToken(token);
                
                if (principal != null)
                {
                    // Adiciona o principal ao contexto
                    context.User = principal;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha na validação do token JWT");
            }
        }

        await _next(context);
    }

    private string? ExtractTokenFromHeader(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authorizationHeader))
            return null;

        // Formato esperado: "Bearer {token}"
        if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        return null;
    }
}
