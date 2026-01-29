using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;
using PrismaPrimeMarket.Infrastructure.Data;
using PrismaPrimeMarket.Infrastructure.Data.Context;
using PrismaPrimeMarket.Infrastructure.Repositories;

namespace PrismaPrimeMarket.CrossCutting.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("PrismaPrimeMarket.Infrastructure")));

        // Configuração do Identity
        services.AddIdentityCore<User>(options =>
        {
            // Configurações de senha
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Configurações de usuário
            options.User.RequireUniqueEmail = false; // Email é opcional
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
