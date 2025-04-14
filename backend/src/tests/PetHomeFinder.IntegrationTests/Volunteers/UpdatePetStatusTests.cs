using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Volunteers.Commands.UpdatePetStatus;
using PetHomeFinder.Domain.PetManagement.Entities;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UpdatePetStatusTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdatePetStatusCommand> _sut;

    public UpdatePetStatusTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetStatusCommand>>();
    }

    [Fact]
    public async Task Update_pet_status_should_work()
    {
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var newStatus = HelpStatusEnum.SEARCH_FOR_HOME;

        var command = new UpdatePetStatusCommand(volunteerId, pet, "SEARCH_FOR_HOME");

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeEmpty();

        var petQuery = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)
            .PetsOwning
            .FirstOrDefault(p => p.Id.Value == pet);

        petQuery.Should().NotBeNull();

        petQuery.HelpStatus.Should().Be(newStatus);
    }
}