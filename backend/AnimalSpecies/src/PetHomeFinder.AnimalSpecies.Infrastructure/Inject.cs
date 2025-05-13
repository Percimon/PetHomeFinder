using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAnimalSpeciesInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        services.AddScoped<SpeciesWriteDbContext>(_ =>
            new SpeciesWriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddScoped<ISpeciesReadDbContext, SpeciesSpeciesReadDbContext>(_ =>
            new SpeciesSpeciesReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(ModuleKey.AnimalSpecies);
        
        return services;
    }
 
}
