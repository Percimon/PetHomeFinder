using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.Delete;

public record DeleteSpeciesCommand(Guid SpeciesId) : ICommand;