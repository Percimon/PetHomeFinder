using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(int Page, int PageSize) : IQuery;