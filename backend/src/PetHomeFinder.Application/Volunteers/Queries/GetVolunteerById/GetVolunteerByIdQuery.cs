using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;