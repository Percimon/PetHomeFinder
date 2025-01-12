using PetHomeFinder.Application.SpeciesBreeds.Commands.AddBreed;

namespace PetHomeFinder.API.Controllers.Species.Requests;

public record AddBreedRequest(string Name)
{
    public AddBreedCommand ToCommand(Guid speciesId) => new (speciesId, Name);
}