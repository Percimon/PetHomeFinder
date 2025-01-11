using PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

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