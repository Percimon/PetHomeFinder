using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Volunteers.Commands.HardDeletePetById;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class HardDeletePetByIdTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, HardDeletePetByIdCommand> _sut;

    public HardDeletePetByIdTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, HardDeletePetByIdCommand>>();
    }
    
    [Fact]
    public async Task Hard_delete_pet_by_id_should_work()
    {
        var volunteerId = await SeedVolunteerAsync();
        
        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        
        var species = await SeedSpeciesAsync();
        
        var breedId = await SeedBreedAsync(species);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new HardDeletePetByIdCommand(volunteerId, pet);
        
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteerPets = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)
            .PetsOwning;
        
        volunteerPets.Should().BeEmpty();
    }
}