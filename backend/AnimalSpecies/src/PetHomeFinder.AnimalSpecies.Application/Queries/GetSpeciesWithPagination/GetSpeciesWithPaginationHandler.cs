using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly ISpeciesReadDbContext _speciesReadDbContext;

    public GetSpeciesWithPaginationHandler(ISpeciesReadDbContext speciesReadDbContext)
    {
        _speciesReadDbContext = speciesReadDbContext;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetSpeciesWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = _speciesReadDbContext.Species;
        
        var result = await speciesQuery.ToPagedList(
            query.Page, 
            query.PageSize, 
            cancellationToken);
        
        return result;
    }
}