using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeMarket.Infrastructure.Data.Context;

namespace PrismaPrimeMarket.IntegrationTests.Setup;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestDatabaseName = "PrismaPrimeMarketDB_Test";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Adiciona configurações de teste
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:AccessSecret"] = "ThisIsAVerySecureSecretKeyForTestingPurposesWithAtLeast32Characters!",
                ["Jwt:RefreshSecret"] = "ThisIsAVerySecureRefreshSecretKeyForTestingPurposesWithAtLeast32Characters!",
                ["Jwt:Issuer"] = "PrismaPrimeMarket.Tests",
                ["Jwt:Audience"] = "PrismaPrimeMarket.Tests",
                ["Jwt:AccessTokenExpirationMinutes"] = "60",
                ["Jwt:RefreshTokenExpirationDays"] = "7"
            });
        });

        builder.ConfigureServices(services =>
        {
            // Remove o DbContext PostgreSQL existente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            // Obtém connection string de teste do ambiente ou usa padrão local
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? $"Host=localhost;Port=5433;Database={TestDatabaseName};Username=postgres;Password=postgres";

            // Adiciona DbContext PostgreSQL para testes
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Garante que o banco de dados é criado e migrado
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Recria o banco para cada execução de teste (garantindo estado limpo)
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}
