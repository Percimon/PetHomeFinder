using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Queries.GetVolunteersWithPagination;

public class
    GetVolunteersWithPaginationHandler : IQueryHandler<PagedList<VolunteerDto>, GetVolunteersWithPaginationQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;

    public GetVolunteersWithPaginationHandler(IVolunteersReadDbContext volunteersReadDbContext)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
    }

    public async Task<Result<PagedList<VolunteerDto>, ErrorList>> Handle(
        GetVolunteersWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var volunteersQuery = _volunteersReadDbContext.Volunteers;

        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.FirstName),
            v => v.FirstName.Contains(query.FirstName));

        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.LastName),
            v => v.LastName.Contains(query.LastName));

        volunteersQuery = volunteersQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Surname),
            v => v.Surname.Contains(query.Surname));

        var result = await volunteersQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);

        return result;
    }
}