using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    internal GetBreedsBySpeciesIdHandler(IReadDbContext readDbContext)
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