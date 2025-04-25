using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Queries.GetSpeciesWithPagination;

public class GetSpeciesWithPaginationHandler : IQueryHandler<PagedList<SpeciesDto>, GetSpeciesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetSpeciesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<SpeciesDto>, ErrorList>> Handle(
        GetSpeciesWithPaginationQuery query, 
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = _readDbContext.Species;
        
        var result = await speciesQuery.ToPagedList(
            query.Page, 
            query.PageSize, 
            cancellationToken);
        
        return result;
    }
}