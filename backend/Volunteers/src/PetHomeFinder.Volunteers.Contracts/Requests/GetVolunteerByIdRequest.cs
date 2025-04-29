namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record GetVolunteerByIdRequest(Guid Id)
{
    // public GetVolunteerByIdQuery ToQuery() => new(Id);
}