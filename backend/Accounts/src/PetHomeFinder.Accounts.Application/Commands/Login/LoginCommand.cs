using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Accounts.Application.Commands.Login;

public record LoginCommand(string Email, string Password): ICommand;