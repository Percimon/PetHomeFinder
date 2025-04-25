using PetHomeFinder.Volunteers.Application.Queries.GetVolunteersWithPagination;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record GetVolunteersWithPaginationRequest(
    string? FirstName,
    string? LastName,
    string? Surname,
    int Page,
    int PageSize)
{
    public GetVolunteersWithPaginationQuery ToQuery() =>
        new(FirstName, LastName, Surname, Page, PageSize);
}