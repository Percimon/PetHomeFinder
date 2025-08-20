using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Accounts.Infrastructure.IdentityManagers;
using PetHomeFinder.Accounts.Infrastructure.Options;
using PetHomeFinder.Framework;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Infrastructure.Seeding;

public class AccountsSeederService
{
    private readonly AdminAccountManager _adminAccountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly PermissionManager _permissionManager;
    private readonly RolePermissionManager _rolePermissionManager;
    private readonly AdminOptions _adminOptions;
    private readonly ILogger<AccountsSeederService> _logger;

    public AccountsSeederService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        PermissionManager permissionManager,
        RolePermissionManager rolePermissionManager,
        IOptions<AdminOptions> adminOptions,
        ILogger<AccountsSeederService> logger,
        AdminAccountManager adminAccountManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _permissionManager = permissionManager;
        _rolePermissionManager = rolePermissionManager;
        _adminOptions = adminOptions.Value;
        _logger = logger;
        _adminAccountManager = adminAccountManager;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding Accounts...");

        var json = await File.ReadAllTextAsync(FilePaths.Accounts);

        var seedData = JsonSerializer.Deserialize<RolePermissionsOptions>(json)
                       ?? throw new Exception("Could not deserialize role permissions config.");

        await SeedPermissions(seedData);

        await SeedRoles(seedData);

        await SeedRolePermissions(seedData);

        await SeedAdminAccount();
    }

    private async Task SeedAdminAccount()
    {
        var adminAccountSearchResult = await _userManager.FindByEmailAsync(_adminOptions.Email);
        if (adminAccountSearchResult is not null)
            return;
        
        var adminRole = await _roleManager.FindByNameAsync(AdminAccount.Admin)
                        ?? throw new Exception("Could not find admin role");
        
        var adminUserResult = User.CreateAdmin(_adminOptions.UserName, _adminOptions.Email, adminRole);
        if (adminUserResult.IsFailure)
            throw new ApplicationException($"Failed to create admin user: {adminUserResult.Error}");

        await _userManager.CreateAsync(adminUserResult.Value, _adminOptions.Password);

        await _userManager.AddToRoleAsync(adminUserResult.Value, AdminAccount.Admin);

        var fullNameResult = FullName.Create(
            _adminOptions.UserName,
            _adminOptions.UserName,
            _adminOptions.UserName);
        
        if (fullNameResult.IsFailure)
            throw new ApplicationException($"Failed to create full name: {fullNameResult.Error}");
        
        var adminAccount = new AdminAccount(fullNameResult.Value, adminUserResult.Value);
        
        await _adminAccountManager.CreateAdminAccount(adminAccount);
    }

    private async Task SeedRolePermissions(RolePermissionsOptions seedData)
    {
        foreach (var roleName in seedData.Roles.Keys)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            var rolePermissions = seedData.Roles[roleName];

            await _rolePermissionManager.AddRange(role.Id, rolePermissions);
        }

        _logger.LogInformation("Role permissions added to database...");
    }

    private async Task SeedRoles(RolePermissionsOptions seedData)
    {
        foreach (var role in seedData.Roles.Keys)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                await _roleManager.CreateAsync(new Role() { Name = role });
            }
        }

        _logger.LogInformation("Roles added to database...");
    }

    private async Task SeedPermissions(RolePermissionsOptions seedData)
    {
        var permissionsToAdd = seedData.Permissions
            .SelectMany(p => p.Value);

        await _permissionManager.AddRange(permissionsToAdd);

        _logger.LogInformation("Permissions added to database...");
    }
}