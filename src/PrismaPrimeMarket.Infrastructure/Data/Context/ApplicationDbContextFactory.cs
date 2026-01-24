using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PrismaPrimeMarket.Infrastructure.Data.Context;

/// <summary>
/// Factory para criar o ApplicationDbContext em design-time (migrations)
/// </summary>
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // Connection string para design-time (pode ser substituída por variável de ambiente)
        var connectionString = Environment.GetEnvironmentVariable("DefaultConnection") 
            ?? throw new InvalidOperationException("A connection string 'DefaultConnection' deve ser fornecida como variável de ambiente para design-time.");
        
        optionsBuilder.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("PrismaPrimeMarket.Infrastructure"));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
