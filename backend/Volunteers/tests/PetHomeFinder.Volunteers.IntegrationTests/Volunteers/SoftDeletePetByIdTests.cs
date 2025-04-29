using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.SoftDeletePetById;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

public class SoftDeletePetByIdTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, SoftDeletePetByIdCommand> _sut;

    public SoftDeletePetByIdTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, SoftDeletePetByIdCommand>>();
    }
    
    [Fact]
    public async Task Soft_delete_pet_by_id_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();
        
        var volunteer = VolunteersWriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        
        var species = await SeedSpeciesAsync();
        
        var breedId = await SeedBreedAsync(species);

        var pet = await SeedPetAsync(volunteer, species, breedId);

        var command = new SoftDeletePetByIdCommand(volunteerId, pet);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
    }
}