using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Create;

namespace PetHomeFinder.IntegrationTests.SpeciesBreeds;

public class CreateSpeciesTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, CreateSpeciesCommand> _sut;

    public CreateSpeciesTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateSpeciesCommand>>();
    }
    
    [Fact]
    public async Task Creates_Species_Should_Be_Successful()
    {
        var command = new CreateSpeciesCommand("Test");
        
        var result = await _sut.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var species = ReadDbContext.Species.SingleOrDefault(s => s.Id == result.Value);
        
        species.Should().NotBeNull();
    }
}