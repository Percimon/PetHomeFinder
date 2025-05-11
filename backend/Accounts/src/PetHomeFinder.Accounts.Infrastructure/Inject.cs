using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AccountDbContext>();
        
        services.AddScoped<AccountDbContext>();
        
        return services;
    }
}