using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

public record GetVolunteersWithPaginationQuery(
    string? FirstName,
    string? LastName,
    string? Surname,
    int Page, 
    int PageSize) : IQuery;