using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Framework.Authorization;

namespace PetHomeFinder.Framework;

public static class Inject
{
    public static IServiceCollection AddFramework(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequiermentHandler>();

        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        
        return services;
    }
}