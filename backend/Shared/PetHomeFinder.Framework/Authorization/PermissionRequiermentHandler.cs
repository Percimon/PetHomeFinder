using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using PetHomeFinder.Framework.Models;

namespace PetHomeFinder.Framework.Authorization;

public class PermissionRequiermentHandler : AuthorizationHandler<PermissionAttribute>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute requirement)
    {
        var permissions = context.User.Claims
            .Where(c => c.Type == CustomClaims.Permission)
            .Select(c => c.Value)
            .ToArray();

        if (permissions.Contains(requirement.Code))
        {
            context.Succeed(requirement);
            return;
        }
        
        context.Fail();
    }
}