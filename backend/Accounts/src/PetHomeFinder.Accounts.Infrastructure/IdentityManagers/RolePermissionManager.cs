using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.IdentityManagers;

public class RolePermissionManager
{
    private readonly AccountsDbContext _context;

    public RolePermissionManager(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task AddRange(Guid roleId, IEnumerable<string> permissions)
    {
        foreach (var permissionCode in permissions)
        {
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Code == permissionCode);
            
            if(permission is null)
                throw new ApplicationException($"Permission with Code {permissionCode} not found");
            
            var rolePermissionsExists = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);
                
            if(rolePermissionsExists)
                continue;

            _context.RolePermissions.Add(new RolePermission()
            {
                RoleId = roleId,
                PermissionId = permission.Id
            }); 
        }

        await _context.SaveChangesAsync();
    }
}