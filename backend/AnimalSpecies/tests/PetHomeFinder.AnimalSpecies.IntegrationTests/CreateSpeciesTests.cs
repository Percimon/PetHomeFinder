using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Application.Commands.Create;
using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.IntegrationTests;

public class CreateSpeciesTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;

    public CreateSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
    }
    
    [Fact]
    public async Task Creates_species_should_work()
    {
        //Arrange
        var command = new CreateSpeciesCommand("Test");
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var species = SpeciesReadDbContext.Species.SingleOrDefault(s => s.Id == result.Value);
        
        species.Should().NotBeNull();
    }
}