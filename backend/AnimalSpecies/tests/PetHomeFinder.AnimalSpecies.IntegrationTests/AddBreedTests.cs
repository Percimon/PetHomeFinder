using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;
using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.IntegrationTests;

public class AddBreedTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, AddBreedCommand> _sut;
    
    public AddBreedTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, AddBreedCommand>>();
    }
    
    [Fact]
    public async Task Add_breed_should_work()
    {
        //Arrange
        var species = await SeedSpeciesAsync();

        var command = new AddBreedCommand(species.Id, "test");
        
        //Act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //Assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
        
        var breed = SpeciesReadDbContext.Breeds.SingleOrDefault(b => b.Id == result.Value);
        
        breed.Should().NotBeNull();
    }
}