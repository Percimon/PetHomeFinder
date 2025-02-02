using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.Models;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetBreedsBySpeciesIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> Handle(
        GetBreedsBySpeciesIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var queryResult = _readDbContext.Breeds
            .Where(b => b.SpeciesId == query.SpeciesId);

        return await queryResult.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}