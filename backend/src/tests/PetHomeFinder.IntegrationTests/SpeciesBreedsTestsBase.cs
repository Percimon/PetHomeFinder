using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.Entities;
using PetHomeFinder.Domain.SpeciesManagement.IDs;
using PetHomeFinder.Infrastructure.DbContexts;

namespace PetHomeFinder.IntegrationTests;

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