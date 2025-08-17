using Microsoft.AspNetCore.Identity;

namespace PetHomeFinder.Accounts.Domain;

public class Role : IdentityRole<Guid>
{
    public List<RolePermission> RolePermissions { get; set; } = [];
}