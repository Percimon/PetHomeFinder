using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetHomeFinder.Accounts.Application;
using PetHomeFinder.Accounts.Application.Abstractions;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Accounts.Infrastructure.IdentityManagers;
using PetHomeFinder.Accounts.Infrastructure.Options;
using PetHomeFinder.Framework.Models;

namespace PetHomeFinder.Accounts.Infrastructure.Providers;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtOptions _options;
    private readonly PermissionManager _permissionManager;

    public JwtTokenProvider(IOptions<JwtOptions> options, PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
        _options = options.Value;
    }

    public async Task<string> GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roleClaims = user.Roles
            .Select(r => new Claim(CustomClaims.Role, r.Name ?? string.Empty));

        var permissions = await _permissionManager.GetUserPermissionCodes(user.Id);
        
        var permissionClaims = permissions
            .Select(p => new Claim(CustomClaims.Permission, p));
        
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
        ];

        claims = claims
            .Concat(roleClaims)
            .Concat(permissionClaims)
            .ToArray(); 

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetime),
            signingCredentials: signingCredentials,
            claims: claims);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }
}