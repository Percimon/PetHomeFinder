using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.Accounts.Application.Commands.Login;
using PetHomeFinder.Accounts.Application.Commands.Register;
using PetHomeFinder.Accounts.Presentation.Requests;
using PetHomeFinder.Framework;
using PetHomeFinder.Framework.Authorization;

namespace PetHomeFinder.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterRequest request,
        [FromServices] RegisterHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.UserName,
            request.Email,
            request.Password);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToResponse();
        }

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Volunteers.CREATE)]
    [HttpPost("test-admin")]
    public async Task<ActionResult> TestAdminAccount()
    {
        return Ok();
    }
    
    [Permission(Permissions.Volunteers.READ)]
    [HttpPost("test-participant")]
    public async Task<ActionResult> TestParticipantAccount()
    {
        return Ok();
    }
}