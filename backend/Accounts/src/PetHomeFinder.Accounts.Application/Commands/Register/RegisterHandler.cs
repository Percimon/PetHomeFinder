using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Accounts.Application.Abstractions;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Application.Commands.Register;

public class RegisterHandler : ICommandHandler<RegisterCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IParticipantAccountManager _participantAccountManager;
    private readonly ILogger<RegisterHandler> _logger;

    public RegisterHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IParticipantAccountManager participantAccountManager,
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _participantAccountManager = participantAccountManager;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken = default)
    {
        var userSearchResult = await _userManager.FindByEmailAsync(command.Email);
        if (userSearchResult is not null)
            return Errors.General.AlreadyExists("Email").ToErrorList();

        var role = await _roleManager.FindByNameAsync(ParticipantAccount.PARTICIPANT);
        if (role == null)
            return Errors.General.ValueIsInvalid("Role").ToErrorList();

        var userResult = User.CreateParticipant(command.UserName, command.Email, role);
        if (userResult.IsFailure)
            return userResult.Error.ToErrorList();

        var result = await _userManager.CreateAsync(userResult.Value, command.Password);
        if (result.Succeeded == false)
        {
            var resultErrors = result.Errors
                .Select(e => Error.Failure(e.Code, e.Description));

            _logger.LogInformation("Failed to create user from username: {Username}", command.UserName);

            return new ErrorList(resultErrors);
        }

        var participantAccount = new ParticipantAccount(userResult.Value);
        
        await _participantAccountManager.CreateParticipantAccount(participantAccount, cancellationToken);

        _logger.LogInformation("User created: {userName} a new account with password", command.UserName);

        return Result.Success<ErrorList>();
    }
}