using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.AddPet;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

public class AddPetTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, AddPetCommand> _sut;

    public AddPetTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddPetCommand>>();
    }

    [Fact]
    public async Task Add_pet_to_database_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var command = Fixture.CreateAddPetCommand(
            volunteerId,
            species,
            breedId);

        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeEmpty();

        var pet = VolunteersReadDbContext.Pets.FirstOrDefault(x => x.Id == result.Value);

        pet.Should().NotBeNull();

        pet.VolunteerId.Should().Be(volunteerId);
    }
}