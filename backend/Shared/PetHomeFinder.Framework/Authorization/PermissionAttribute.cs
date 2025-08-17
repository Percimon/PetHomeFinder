using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetHomeFinder.Framework.Authorization;

public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public PermissionAttribute(string code) 
        : base(policy: code)
    {
        Code = code;
    }
    
    public string Code { get; set; }
}