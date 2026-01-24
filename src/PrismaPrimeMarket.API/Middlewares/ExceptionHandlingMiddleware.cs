using System.Net;
using System.Text.Json;
using PrismaPrimeMarket.Application.Common.Models;
using PrismaPrimeMarket.Domain.Exceptions;

namespace PrismaPrimeMarket.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An error occurred while processing the request");

        var response = context.Response;
        response.ContentType = "application/json";
        
        Response<string> responseModel;
        var path = context.Request.Path.Value;

        switch (exception)
        {
            case DomainException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel = Response<string>.BadRequest(exception.Message, path: path);
                break;
            case FluentValidation.ValidationException validationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = validationException.Errors.Select(e => e.ErrorMessage).ToArray();
                responseModel = Response<string>.ValidationError(errors, path: path);
                break;
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                responseModel = Response<string>.NotFound(exception.Message, path: path);
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                responseModel = Response<string>.InternalServerError(path: path);
                break;
        }

        var result = JsonSerializer.Serialize(responseModel);
        await response.WriteAsync(result);
    }
}
