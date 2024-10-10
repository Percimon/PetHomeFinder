using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Volunteers.Create;

namespace PetHomeFinder.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
}
