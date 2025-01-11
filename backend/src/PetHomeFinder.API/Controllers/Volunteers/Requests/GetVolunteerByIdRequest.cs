using PetHomeFinder.Application.Volunteers.Queries.GetVolunteerById;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

public record GetVolunteerByIdRequest(Guid Id)
{
    public GetVolunteerByIdQuery ToQuery() => new(Id);
}