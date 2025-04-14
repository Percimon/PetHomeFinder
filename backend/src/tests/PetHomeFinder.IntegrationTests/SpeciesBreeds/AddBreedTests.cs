using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.SpeciesBreeds.Commands.AddBreed;

namespace PetHomeFinder.IntegrationTests.SpeciesBreeds;

public class AddBreedTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;
    
    public AddBreedTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }
    
    [Fact]
    public async Task Add_Breed_Should_Be_Successful()
    {
        var species = await SeedSpeciesAsync();

        var command = new AddBreedCommand(species.Id, "test");
        
        var result = await _sut.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
        
        var breed = ReadDbContext.Breeds.SingleOrDefault(b => b.Id == result.Value);
        
        breed.Should().NotBeNull();
    }
}