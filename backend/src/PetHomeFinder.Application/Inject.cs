using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Volunteers.Create;
using PetHomeFinder.Application.Volunteers.Delete;
using PetHomeFinder.Application.Volunteers.UpdateCredentials;
using PetHomeFinder.Application.Volunteers.UpdateMainInfo;
using PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

namespace PetHomeFinder.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateCredentialsHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}
