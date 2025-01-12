using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.Create;

public record CreateSpeciesCommand(string Name) : ICommand;