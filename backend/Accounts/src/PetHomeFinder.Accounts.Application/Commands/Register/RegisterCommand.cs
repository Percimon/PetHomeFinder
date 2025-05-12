using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Accounts.Application.Commands.Register;

public record RegisterCommand(
    string UserName, 
    string Email, 
    string Password) : ICommand;