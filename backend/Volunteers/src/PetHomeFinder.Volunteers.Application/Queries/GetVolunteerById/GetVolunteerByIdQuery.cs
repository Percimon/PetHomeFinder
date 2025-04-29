using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Queries.GetVolunteerById;

public record GetVolunteerByIdQuery(Guid VolunteerId) : IQuery;