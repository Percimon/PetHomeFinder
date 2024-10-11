using System;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Volunteers;
using PetHomeFinder.Infrastructure.Repositories;

namespace PetHomeFinder.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();

        return services;
    }
}
