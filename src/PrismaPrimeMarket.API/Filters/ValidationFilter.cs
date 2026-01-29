using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PrismaPrimeMarket.Application.Common.Models;

namespace PrismaPrimeMarket.API.Filters;

public class ValidationFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .ToArray();

            var response = Response<string>.ValidationError(
                errors,
                path: context.HttpContext.Request.Path.Value
            );

            context.Result = new BadRequestObjectResult(response);
        }
        base.OnActionExecuting(context);
    }
}
