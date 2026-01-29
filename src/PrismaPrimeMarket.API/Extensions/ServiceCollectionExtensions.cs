using PrismaPrimeMarket.API.Filters;
using PrismaPrimeMarket.API.Middlewares;

namespace PrismaPrimeMarket.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true; // Disable default model validation to use our custom filter
        });

        // Add other presentation layer services here

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        // Add request logging middleware here if created

        return app;
    }
}
