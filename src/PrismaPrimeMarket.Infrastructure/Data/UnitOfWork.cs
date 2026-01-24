using PrismaPrimeMarket.Domain.Interfaces;
using PrismaPrimeMarket.Infrastructure.Data.Context;

namespace PrismaPrimeMarket.Infrastructure.Data;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
