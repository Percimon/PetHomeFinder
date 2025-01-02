using CSharpFunctionalExtensions;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.Models;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetVolunteersWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _readDbContext.Volunteers;
        
        var result = await volunteersQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
        
        return result;
    }
}