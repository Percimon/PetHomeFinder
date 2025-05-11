using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.Accounts.Application.Commands.RegisterUser;

public record RegisterUserCommand(
    string UserName, 
    string Email, 
    string Password) : ICommand;