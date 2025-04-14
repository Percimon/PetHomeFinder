using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Volunteers.Commands.UpdatePet;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UpdatePetTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdatePetCommand> _sut;

    public UpdatePetTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetCommand>>();
    }

    [Fact]
    public async Task Update_pet_should_work()
    {
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers
            .Include(v => v.PetsOwning)
            .ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var newDescription = "new-pet-description";

        var command = Fixture.CreateUpdatePetCommand(
            volunteerId,
            pet,
            species.Id,
            breedId,
            newDescription);
        
        var result = await _sut.Handle(command);   
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
        
        var petQuery = WriteDbContext.Volunteers
            .ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId)
            .PetsOwning
            .FirstOrDefault(p => p.Id.Value == pet);
        
        petQuery.Description.Value.Should().Be(newDescription);
    }
}