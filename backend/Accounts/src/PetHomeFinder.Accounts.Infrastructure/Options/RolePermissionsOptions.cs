namespace PetHomeFinder.Accounts.Infrastructure.Options;

public class RolePermissionsOptions
{
    public Dictionary<string, string[]> Permissions { get; set; } = [];

    public Dictionary<string, string[]> Roles { get; set; } = [];
}