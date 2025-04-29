using Serilog;

namespace PetHomeFinder.Web;

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
