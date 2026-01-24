namespace PrismaPrimeMarket.Application.Common.Models;

public class Response<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string[]? Errors { get; set; }
    public string Message { get; set; } = string.Empty;
    public ResponseType Type { get; set; }
    public DateTime Timestamp { get; set; }
    public string? Path { get; set; }

    public Response()
    {
        Timestamp = DateTime.UtcNow;
    }

    // Factory methods for success responses
    public static Response<T> Success(T data, string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = data,
            Succeeded = true,
            Message = customMessage ?? ResponseMessages.Success,
            Type = ResponseType.Success,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Created(T data, string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = data,
            Succeeded = true,
            Message = customMessage ?? ResponseMessages.Created,
            Type = ResponseType.Created,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Updated(T data, string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = data,
            Succeeded = true,
            Message = customMessage ?? ResponseMessages.Updated,
            Type = ResponseType.Updated,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Deleted(string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = true,
            Message = customMessage ?? ResponseMessages.Deleted,
            Type = ResponseType.Deleted,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Retrieved(T data, string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = data,
            Succeeded = true,
            Message = customMessage ?? ResponseMessages.Retrieved,
            Type = ResponseType.Retrieved,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    // Factory methods for error responses
    public static Response<T> NotFound(string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.NotFound,
            Type = ResponseType.NotFound,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> ValidationError(string[] errors, string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.ValidationError,
            Type = ResponseType.ValidationError,
            Errors = errors,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> BadRequest(string? customMessage = null, string[]? errors = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.BadRequest,
            Type = ResponseType.BadRequest,
            Errors = errors,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Unauthorized(string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.Unauthorized,
            Type = ResponseType.Unauthorized,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Forbidden(string? customMessage = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.Forbidden,
            Type = ResponseType.Forbidden,
            Errors = null,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> Conflict(string? customMessage = null, string[]? errors = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.Conflict,
            Type = ResponseType.Conflict,
            Errors = errors,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }

    public static Response<T> InternalServerError(string? customMessage = null, string[]? errors = null, string? path = null)
    {
        return new Response<T>
        {
            Data = default,
            Succeeded = false,
            Message = customMessage ?? ResponseMessages.InternalServerError,
            Type = ResponseType.InternalServerError,
            Errors = errors,
            Path = path,
            Timestamp = DateTime.UtcNow
        };
    }
}
