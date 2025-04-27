using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Application.Commands.Delete;
using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.IntegrationTests;

public class DeleteSpeciesTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;

    public DeleteSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
    }

    [Fact]
    public async Task Delete_species_should_work()
    {
        //Arrange
        var species = await SeedSpeciesAsync();

        var command = new DeleteSpeciesCommand(species.Id);

        //Act
        var result = await _sut.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();

        var speciesQuery = SpeciesReadDbContext.Species.ToList();

        speciesQuery.Count.Should().Be(0);
    }
}