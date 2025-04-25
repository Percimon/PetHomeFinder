using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name) : ICommand;