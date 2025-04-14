using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Volunteers.Commands.SoftDeletePetById;

namespace PetHomeFinder.IntegrationTests.Volunteers;

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
        var volunteerId = await SeedVolunteerAsync();
        
        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId);
        
        var species = await SeedSpeciesAsync();
        
        var breedId = await SeedBreedAsync(species);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var command = new SoftDeletePetByIdCommand(volunteerId, pet);
        
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
    }
}