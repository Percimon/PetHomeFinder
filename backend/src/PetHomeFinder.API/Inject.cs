using System;
using Serilog;

namespace PetHomeFinder.API;

public static class Inject
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSwaggerGen();
        services.AddSerilog();
        return services;
    }
}
