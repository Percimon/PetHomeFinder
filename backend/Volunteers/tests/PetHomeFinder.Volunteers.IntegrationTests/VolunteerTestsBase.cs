using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Contracts;
using PetHomeFinder.AnimalSpecies.Contracts.Requests;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Domain.Entities;
using PetHomeFinder.Volunteers.Domain.ValueObjects;
using PetHomeFinder.Volunteers.Infrastructure.DbContexts;

namespace PetHomeFinder.Volunteers.IntegrationTests;

public class VolunteerTestsBase : IClassFixture<VolunteerTestsWebFactory>, IAsyncLifetime
{
    protected readonly Fixture Fixture;
    protected readonly VolunteersWriteDbContext VolunteersWriteDbContext;
    protected readonly IVolunteersReadDbContext VolunteersReadDbContext;
    protected readonly IServiceScope Scope;
    protected readonly VolunteerTestsWebFactory Factory;
    protected readonly IAnimalSpeciesContract AnimalSpeciesContract;

    protected VolunteerTestsBase(VolunteerTestsWebFactory factory)
    {
        Factory = factory;
        Scope = factory.Services.CreateScope();
        VolunteersReadDbContext = Scope.ServiceProvider.GetRequiredService<IVolunteersReadDbContext>();
        VolunteersWriteDbContext = Scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        AnimalSpeciesContract = Scope.ServiceProvider.GetRequiredService<IAnimalSpeciesContract>();
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

        await VolunteersWriteDbContext.Volunteers.AddAsync(volunteer);

        await VolunteersWriteDbContext.SaveChangesAsync(CancellationToken.None);

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
        var address = Address.Create(
            "test-city",
            "test-district",
            "test-street",
            "test-structure").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("123456789").Value;
        var isVaccinated = true;
        var isCastrated = true;

        DateTime dateOfBirth = DateTime.UtcNow;

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

        var photos = new List<PetPhoto>
        {
            PetPhoto.Create("testFile-1.jpg", true).Value,
            PetPhoto.Create("testFile-2.jpg").Value,
            PetPhoto.Create("testFile-3.jpg").Value,
        };

        pet.UpdatePhotos(photos);

        await VolunteersWriteDbContext.SaveChangesAsync(CancellationToken.None);

        return id.Value;
    }

    public async Task SeedNPetsAsync(Volunteer volunteer,
        Guid speciesId,
        Guid breedId,
        int petsCount)
    {
        var speciesBreed = SpeciesBreed.Create(speciesId, breedId).Value;

        DateTime dateOfBirth = DateTime.UtcNow;

        var birthDate = dateOfBirth;

        var createDate = dateOfBirth;

        for (var i = 0; i < petsCount; i++)
        {
            var id = PetId.New();
            var name = Name.Create($"test-pet-{i}").Value;
            var description = Description.Create("test").Value;
            var color = Color.Create($"test-color-{i}").Value;
            var health = HealthInfo.Create("test").Value;
            var address = Address.Create(
                "test-city",
                "test-district",
                "test-street",
                "test-structure").Value;
            var weight = Weight.Create(i + 1).Value;
            var height = Height.Create(i + 1).Value;
            var phoneNumber = PhoneNumber.Create("123456789").Value;
            var isVaccinated = true;
            var isCastrated = true;

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
            
            var photos = new List<PetPhoto>
            {
                PetPhoto.Create("testFile-1.jpg", true).Value,
                PetPhoto.Create("testFile-2.jpg").Value,
                PetPhoto.Create("testFile-3.jpg").Value,
            };

            pet.UpdatePhotos(photos);
        }

        await VolunteersWriteDbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<Guid> SeedSpeciesAsync()
    {
        var speciesName = "test-species";

        var request = new CreateSpeciesRequest(speciesName);

        var result = await AnimalSpeciesContract.CreateSpecies(request, CancellationToken.None);

        return result.Value;
    }

    public async Task<Guid> SeedBreedAsync(Guid speciesId)
    {
        var breedName = "test-breed";

        var request = new AddBreedRequest(breedName);

        var result = await AnimalSpeciesContract.AddBreed(speciesId, request, CancellationToken.None);

        return result.Value;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Scope.Dispose();

        await Factory.ResetDatabaseAsync();
    }
}