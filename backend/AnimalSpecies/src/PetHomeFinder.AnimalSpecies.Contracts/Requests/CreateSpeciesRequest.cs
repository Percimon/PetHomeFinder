using PetHomeFinder.AnimalSpecies.Application.Commands.Create;

namespace PetHomeFinder.AnimalSpecies.Contracts.Requests;

public record CreateSpeciesRequest(string Name)
{
    public CreateSpeciesCommand ToCommand() => new(Name);
}