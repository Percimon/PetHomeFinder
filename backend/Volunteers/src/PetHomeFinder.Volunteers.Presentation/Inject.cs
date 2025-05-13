using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Volunteers.Contracts;

namespace PetHomeFinder.Volunteers.Presentation;

public static class Inject
{
    public static IServiceCollection AddVolunteersPresentation(this IServiceCollection services)
    {
        services.AddScoped<IPetsContract, PetsContract>();
        
        services.AddScoped<IVolunteersContract, VolunteersContract>();
        
        return services;
    }
}