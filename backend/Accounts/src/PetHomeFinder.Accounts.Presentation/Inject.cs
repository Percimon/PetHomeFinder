using Microsoft.Extensions.DependencyInjection;

namespace PetHomeFinder.Accounts.Presentation;

public static class Inject
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        return services;
    }
}