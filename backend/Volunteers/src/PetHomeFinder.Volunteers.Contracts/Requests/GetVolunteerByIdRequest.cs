using PetHomeFinder.Volunteers.Application.Queries.GetVolunteerById;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record GetVolunteerByIdRequest(Guid Id)
{
    public GetVolunteerByIdQuery ToQuery() => new(Id);
}