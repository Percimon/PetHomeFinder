using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    string? FirstName,
    string? LastName,
    string? Surname,
    int Page, 
    int PageSize) : IQuery;