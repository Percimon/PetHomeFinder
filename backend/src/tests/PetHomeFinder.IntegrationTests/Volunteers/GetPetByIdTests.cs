using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Queries.GetPetById;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class GetPetByIdTests  : VolunteerTestsBase
{
    private readonly IQueryHandler<PetDto, GetPetByIdQuery> _sut;
    
    public GetPetByIdTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<PetDto, GetPetByIdQuery>>();
    }

    [Fact]
    public async Task Get_pet_by_id_should_work()
    { 
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);
        
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var query = new GetPetByIdQuery(pet);
        
        //act
        var result = await _sut.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Id.Should().Be(pet);
    }
    
    [Fact]
    public async Task Get_pet_by_id_should_fail()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);
        
        var pet = await SeedPetAsync(volunteer, species.Id, breedId);
        
        var query = new GetPetByIdQuery(Guid.Empty);
        
        //act
        var result = await _sut.Handle(query, CancellationToken.None);

        //assert
        result.IsFailure.Should().BeTrue();
    }
}