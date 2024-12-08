namespace PetHomeFinder.Application.SpeciesBreeds.AddBreed;

public record AddBreedCommand(Guid SpeciesId, string Name);