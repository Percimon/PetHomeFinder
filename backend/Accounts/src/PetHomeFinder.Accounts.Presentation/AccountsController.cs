using Microsoft.AspNetCore.Mvc;
using PetHomeFinder.Accounts.Application.Commands.RegisterUser;
using PetHomeFinder.Accounts.Presentation.Requests;
using PetHomeFinder.Framework;

namespace PetHomeFinder.Accounts.Presentation;

public class AccountsController : ApplicationController
{
    [HttpPost("registration")]
    public async Task<ActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
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
    public async Task<ActionResult> Login()
    {
        return Ok();
    }
}