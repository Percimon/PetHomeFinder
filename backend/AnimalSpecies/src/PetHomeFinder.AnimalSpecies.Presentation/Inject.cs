using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Contracts;

namespace PetHomeFinder.AnimalSpecies.Presentation;

public static class Inject
{
    public static IServiceCollection AddAnimalSpeciesPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAnimalSpeciesContract, AnimalSpeciesContract>();
        
        return services;
    }
}