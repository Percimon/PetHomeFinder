using PetHomeFinder.Application.SpeciesBreeds.Create;

namespace PetHomeFinder.API.Controllers.Species.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() => new(Name);
}