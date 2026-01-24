using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Infrastructure.Data;
using PrismaPrimeMarket.Infrastructure.Data.Context;

namespace PrismaPrimeMarket.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("PrismaPrimeMarket.Infrastructure")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
