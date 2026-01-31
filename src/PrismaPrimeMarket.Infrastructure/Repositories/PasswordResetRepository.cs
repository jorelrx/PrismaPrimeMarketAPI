using Microsoft.EntityFrameworkCore;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;
using PrismaPrimeMarket.Infrastructure.Data.Context;

namespace PrismaPrimeMarket.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de Password Resets
/// </summary>
public class PasswordResetRepository(ApplicationDbContext context) : BaseRepository<PasswordReset>(context), IPasswordResetRepository
{
    public async Task<PasswordReset?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _context.Set<PasswordReset>()
            .FirstOrDefaultAsync(pr => pr.Token == token, cancellationToken);
    }

    public async Task<IEnumerable<PasswordReset>> GetValidResetsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<PasswordReset>()
            .Where(pr => pr.UserId == userId && !pr.IsUsed && pr.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }

    public async Task InvalidateAllUserResetsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var resets = await _context.Set<PasswordReset>()
            .Where(pr => pr.UserId == userId && !pr.IsUsed && pr.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var reset in resets)
        {
            reset.MarkAsUsed();
        }
    }

    public async Task RemoveExpiredResetsAsync(CancellationToken cancellationToken = default)
    {
        var expiredResets = await _context.Set<PasswordReset>()
            .Where(pr => pr.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        _context.Set<PasswordReset>().RemoveRange(expiredResets);
    }
}
