using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.IdentityManagers;

public class PermissionManager
{
    private readonly AccountsDbContext _context;

    public PermissionManager(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task AddRange(IEnumerable<string> permissionsToAdd)
    {
        foreach (var permissionCode in permissionsToAdd)
        {
            var isPermissionExist = await _context.Permissions
                .AnyAsync(p => p.Code == permissionCode);

            if (isPermissionExist)
            {
                continue;
            }

            _context.Permissions.Add(new Permission() { Code = permissionCode });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<HashSet<string>> GetUserPermissionCodes(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .SelectMany(ur => _context.RolePermissions
                .Where(rp => rp.RoleId == ur.RoleId)
                .Select(rp => rp.Permission.Code))
            .AsNoTracking()
            .ToHashSetAsync(cancellationToken);
    }
}