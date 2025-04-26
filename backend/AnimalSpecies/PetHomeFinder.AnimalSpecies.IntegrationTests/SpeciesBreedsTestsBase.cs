using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Domain.Entities;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.AnimalSpecies.IntegrationTests;

public class SpeciesBreedsTestsBase : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly IServiceScope Scope;
    protected readonly IntegrationTestsWebFactory Factory;
    
    protected SpeciesBreedsTestsBase(IntegrationTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
    }
    
    public async Task<Species> SeedSpeciesAsync()
    {
        var speciesId = SpeciesId.New();
        var speciesName = Name.Create("test-species").Value;
        
        var species = new Species(speciesId, speciesName);
        
        await WriteDbContext.Species.AddAsync(species);
        
        await WriteDbContext.SaveChangesAsync(CancellationToken.None);

        return species;
    }

    public async Task<Guid> SeedBreedAsync(Species species)
    {
        var breedId = BreedId.New();
        
        var breedName = Name.Create("test-breed").Value;
        
        var breed = new Breed(breedId, breedName);
        
        species.AddBreed(breed);
        
        await WriteDbContext.SaveChangesAsync(CancellationToken.None);
        
        return breedId.Value;
    } 
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();
        await Factory.InitializeAsync();
    }
}