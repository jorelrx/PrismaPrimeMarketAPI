using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeMarket.Domain.Entities;
using PrismaPrimeMarket.Domain.Interfaces.Repositories;
using PrismaPrimeMarket.Infrastructure.Data.Context;

namespace PrismaPrimeMarket.Infrastructure.Repositories;

/// <summary>
/// Repositório para a entidade User
/// </summary>
public class UserRepository(
    ApplicationDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager) : BaseRepository<User>(context), IUserRepository
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted, cancellationToken);
    }

    public async Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.CPF != null && u.CPF.Number == cpf && !u.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(u => u.UserName == userName && !u.IsDeleted, cancellationToken);
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .AnyAsync(u => u.CPF != null && u.CPF.Number == cpf && !u.IsDeleted, cancellationToken);
    }

    public async Task<List<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(u => u.IsActive && !u.IsDeleted)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetUsersByRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
            return new List<User>();

        var userIds = await _context.UserRoles
            .Where(ur => ur.RoleId == role.Id)
            .Select(ur => ur.UserId)
            .ToListAsync(cancellationToken);

        return await _dbSet
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id) && !u.IsDeleted)
            .OrderBy(u => u.FirstName)
            .ThenBy(u => u.LastName)
            .ToListAsync(cancellationToken);
    }
}
