using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Accounts.Application.Commands.Register;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Application.Commands.Login;

public class LoginHandler : ICommandHandler<string, LoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<RegisterHandler> _logger;

    public LoginHandler(
        UserManager<User> userManager,
        ITokenProvider tokenProvider,
        ILogger<RegisterHandler> logger)
    {
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    public async Task<Result<string, ErrorList>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Errors.General.NotFound().ToErrorList();
        }

        var passwordConfirmed = await _userManager.CheckPasswordAsync(user, command.Password);
        if (passwordConfirmed == false)
        {
            return Errors.User.InvalidCredentials().ToErrorList();
        }

        var token = _tokenProvider.GenerateToken(user);

        _logger.LogInformation("User {Email} logged in.", user.Email);
        
        return token;
    }
}