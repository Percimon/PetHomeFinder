using PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;

namespace PetHomeFinder.AnimalSpecies.Contracts.Requests;

public record AddBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid speciesId) => new (speciesId, Name);
}