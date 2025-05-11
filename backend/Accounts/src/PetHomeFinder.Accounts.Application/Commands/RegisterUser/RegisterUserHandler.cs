using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Application.Commands.RegisterUser;

public class RegisterUserHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        UserManager<User> userManager, 
        ILogger<RegisterUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var user = new User()
        {
            UserName = command.UserName,
            Email = command.Email,
        };
        
        var result = await _userManager.CreateAsync(user, command.Password);
        if (result.Succeeded)
        {
            _logger.LogInformation("User created a new account.");
            return Result.Success<ErrorList>();
        }

        var errors = result.Errors
            .Select(e => Error.Failure(e.Code, e.Description))
            .ToList();
        
        return new ErrorList(errors);
    }
}