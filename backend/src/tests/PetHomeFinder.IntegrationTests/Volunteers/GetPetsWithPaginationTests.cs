using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Models;
using PetHomeFinder.Application.Volunteers.Commands.AddPet;
using PetHomeFinder.Application.Volunteers.Queries.GetPetsWithPagination;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class GetPetsWithPaginationTests : VolunteerTestsBase
{
    private readonly IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery> _sut;

    public GetPetsWithPaginationTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>>();
    }

    [Fact]
    public async Task Get_pets_with_pagination_tests_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        int petTotal = 20;
        int pageSize = 10;

        await SeedNPetsAsync(volunteer, species.Id, breedId, petTotal);

        var nameForTest = "test-pet-3";

        var query = new GetPetsWithPaginationQuery(
            volunteer.Id,
            species.Id,
            breedId,
            nameForTest,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            1,
            pageSize);

        //act
        var result = await _sut.Handle(query, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        var pageList = result.Value;

        pageList.Items.Count.Should().Be(1);

        pageList.PageSize.Should().Be(pageSize);

        var pet = result.Value.Items.FirstOrDefault();

        pet.VolunteerId.Should().Be(volunteerId);

        pet.SpeciesId.Should().Be(species.Id);

        pet.BreedId.Should().Be(breedId);

        pet.Name.Should().Be(nameForTest);
    }
}