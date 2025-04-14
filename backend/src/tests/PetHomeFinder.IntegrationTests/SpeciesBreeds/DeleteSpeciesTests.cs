using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Create;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Delete;

namespace PetHomeFinder.IntegrationTests.SpeciesBreeds;

public class DeleteSpeciesTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, DeleteSpeciesCommand> _sut;

    public DeleteSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteSpeciesCommand>>();
    }

    [Fact]
    public async Task Delete_Species_Should_Be_Successful()
    {
        var species = await SeedSpeciesAsync();

        var command = new DeleteSpeciesCommand(species.Id);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var speciesQuery = ReadDbContext.Species;

        speciesQuery.Should().BeEmpty();
    }
}