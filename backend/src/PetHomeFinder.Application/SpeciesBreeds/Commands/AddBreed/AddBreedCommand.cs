using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;