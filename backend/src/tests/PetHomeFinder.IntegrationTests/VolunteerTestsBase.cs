using System.Diagnostics.Contracts;
using System.Globalization;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.Entities;
using PetHomeFinder.Domain.SpeciesManagement.IDs;
using PetHomeFinder.Infrastructure.DbContexts;

namespace PetHomeFinder.IntegrationTests;

public class VolunteerTestsBase : IClassFixture<VolunteerTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly WriteDbContext WriteDbContext;
    protected readonly IReadDbContext ReadDbContext;
    protected readonly IServiceScope Scope;
    protected readonly VolunteerTestsWebFactory Factory;

    protected VolunteerTestsBase(VolunteerTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        ReadDbContext = Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        WriteDbContext = Scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        Fixture = new Fixture();
    }

    public async Task<Guid> SeedVolunteerAsync()
    {
        var volunteer = new Volunteer(
            VolunteerId.New(),
            FullName.Create("Ivan", "Ivanov", "Ivanovich").Value,
            Description.Create("test-description").Value,
            Experience.Create(1).Value,
            PhoneNumber.Create("123456789").Value,
            Enumerable.Empty<Credential>(),
            Enumerable.Empty<SocialNetwork>());

        await WriteDbContext.Volunteers.AddAsync(volunteer);

        await WriteDbContext.SaveChangesAsync(CancellationToken.None);

        return volunteer.Id.Value;
    }

    public async Task<Guid> SeedPetAsync(Volunteer volunteer,
        Guid speciesId,
        Guid breedId)
    {
        var id = PetId.New();
        var name = Name.Create("test-pet").Value;
        var speciesBreed = SpeciesBreed.Create(speciesId, breedId).Value;
        var description = Description.Create("test").Value;
        var color = Color.Create("test").Value;
        var health = HealthInfo.Create("test").Value;
        var address = Address.Create("test", "test", "test", "test").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("123456789").Value;
        var isVaccinated = true;
        var isCastrated = true;
        
        DateTime dateOfBirth = DateTime.Parse(
            "2025-03-12T13:13:14.384Z",
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal);
        
        var birthDate = dateOfBirth;
        var createDate = dateOfBirth;
        
        var status = HelpStatusEnum.NEED_TREATMENT;
        var petCredentials = new List<Credential>();

        var pet = new Pet(
            id,
            name,
            speciesBreed,
            description,
            color,
            health,
            address,
            weight,
            height,
            phoneNumber,
            isCastrated,
            isVaccinated,
            birthDate,
            status,
            petCredentials,
            createDate);

        volunteer.AddPet(pet);

        await WriteDbContext.SaveChangesAsync(CancellationToken.None);

        return id.Value;
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

        await Factory.ResetDatabaseAsync();
    }
}