using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;