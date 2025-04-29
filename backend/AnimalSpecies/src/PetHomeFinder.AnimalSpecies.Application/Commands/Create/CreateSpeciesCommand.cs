using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Create;

public record CreateSpeciesCommand(string Name) : ICommand;