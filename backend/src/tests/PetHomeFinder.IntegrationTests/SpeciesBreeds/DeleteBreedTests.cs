using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.SpeciesBreeds.Commands.DeleteBreed;

namespace PetHomeFinder.IntegrationTests.SpeciesBreeds;

public class DeleteBreedTests : SpeciesBreedsTestsBase
{
    private readonly ICommandHandler<Guid, DeleteBreedCommand> _sut;

    public DeleteBreedTests(IntegrationTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteBreedCommand>>();
    }
    
    [Fact]
    public async Task Delete_Breed_Should_Be_Successful()
    {
        var species = await SeedSpeciesAsync();
        
        var breedId = await SeedBreedAsync(species);
        
        var command = new DeleteBreedCommand(species.Id, breedId);
        
        var result = await _sut.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().Be(breedId);

        var s = WriteDbContext.Species.ToList();
        
        var breedQuery = s
            .FirstOrDefault(s => s.Id.Value == species.Id.Value)
            .Breeds;
        
        breedQuery.Count.Should().Be(0);
    }
}