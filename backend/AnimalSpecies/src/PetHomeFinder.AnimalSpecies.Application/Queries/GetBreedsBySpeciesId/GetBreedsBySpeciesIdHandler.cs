using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetBreedsBySpeciesId;

public class GetBreedsBySpeciesIdHandler : IQueryHandler<PagedList<BreedDto>, GetBreedsBySpeciesIdQuery>
{
    private readonly ISpeciesReadDbContext _speciesReadDbContext;

    public GetBreedsBySpeciesIdHandler(ISpeciesReadDbContext speciesReadDbContext)
    {
        _speciesReadDbContext = speciesReadDbContext;
    }

    public async Task<Result<PagedList<BreedDto>, ErrorList>> Handle(
        GetBreedsBySpeciesIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var queryResult = _speciesReadDbContext.Breeds
            .Where(b => b.SpeciesId == query.SpeciesId);

        return await queryResult.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}