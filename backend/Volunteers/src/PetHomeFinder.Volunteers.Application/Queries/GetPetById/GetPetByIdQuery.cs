using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Volunteers.Application.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;