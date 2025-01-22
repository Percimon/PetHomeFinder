using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.Models;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.Queries.GetSpeciesWithPagination;

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