using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetHomeFinder.Accounts.Application;
using PetHomeFinder.Accounts.Application.Abstractions;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.Accounts.Infrastructure.IdentityManagers;
using PetHomeFinder.Accounts.Infrastructure.Options;
using PetHomeFinder.Accounts.Infrastructure.Providers;
using PetHomeFinder.Accounts.Infrastructure.Seeding;

namespace PetHomeFinder.Accounts.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddProviders()
            .AddConfigurations(configuration)
            .AddDatabase()
            .RegisterIdentity();
            

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var jwtOptions = configuration.GetSection(JwtOptions.JWT).Get<JwtOptions>()
                                 ?? throw new ApplicationException("Missing JWT configuration");

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
            });
        
        services.AddSingleton<AccountsSeeder>();

        services.AddScoped<AccountsSeederService>();
        
        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddConfigurations(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));

        services.AddOptions<JwtOptions>();
        
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));
        
        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<AccountsDbContext>();
        
        return services;
    }

    private static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AccountsDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<PermissionManager>();
        
        services.AddScoped<RolePermissionManager>();

        services.AddScoped<AdminAccountManager>();
        
        services.AddScoped<IVolunteerAccountManager, VolunteerAccountManager>();
        
        services.AddScoped<IParticipantAccountManager, ParticipantAccountManager>();
        
        return services;
    } 
}