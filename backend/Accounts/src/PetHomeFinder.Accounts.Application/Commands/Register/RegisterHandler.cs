using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Application.Commands.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(
        UserManager<User> userManager, 
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterCommand command, 
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