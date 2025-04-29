using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.HardDeletePetById;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

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
        //arrange
        var volunteerId = await SeedVolunteerAsync();
        
        var volunteer = VolunteersWriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        
        var species = await SeedSpeciesAsync();
        
        var breedId = await SeedBreedAsync(species);

        var pet = await SeedPetAsync(volunteer, species, breedId);

        var command = new HardDeletePetByIdCommand(volunteerId, pet);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteerPets = VolunteersWriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)
            .PetsOwning;
        
        volunteerPets.Should().BeEmpty();
    }
}