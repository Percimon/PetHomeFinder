namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record GetVolunteersWithPaginationRequest(
    string? FirstName,
    string? LastName,
    string? Surname,
    int Page,
    int PageSize);