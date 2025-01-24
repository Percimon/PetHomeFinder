using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.DeleteBreed;

public record DeleteBreedCommand(Guid SpeciesId, Guid BreedId) : ICommand;