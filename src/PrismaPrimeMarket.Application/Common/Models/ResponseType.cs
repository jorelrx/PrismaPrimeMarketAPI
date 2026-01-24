namespace PrismaPrimeMarket.Application.Common.Models;

/// <summary>
/// Tipos de resposta padrão da API
/// </summary>
public enum ResponseType
{
    // Success responses
    Success,
    Created,
    Updated,
    Deleted,
    Retrieved,
    
    // Error responses
    NotFound,
    ValidationError,
    BadRequest,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError
}
