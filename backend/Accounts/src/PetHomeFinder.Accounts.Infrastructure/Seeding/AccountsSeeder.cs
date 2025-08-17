using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PetHomeFinder.Accounts.Infrastructure.Seeding;

public class AccountsSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public AccountsSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    } 

    public async Task SeedAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        
        var seederService = scope.ServiceProvider.GetRequiredService<AccountsSeederService>();
        
        await seederService.SeedAsync();
    }
}