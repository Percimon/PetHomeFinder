using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;