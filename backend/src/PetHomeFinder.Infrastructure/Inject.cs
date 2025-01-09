using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Messaging;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Application.Volunteers;
using PetHomeFinder.Infrastructure.BackgroundServices;
using PetHomeFinder.Infrastructure.DbContexts;
using PetHomeFinder.Infrastructure.Files;
using PetHomeFinder.Infrastructure.MessageQueues;
using PetHomeFinder.Infrastructure.Options;
using PetHomeFinder.Infrastructure.Providers;
using PetHomeFinder.Infrastructure.Repositories;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase()
            .AddRepositories()
            .AddMinio(configuration)
            .AddServices()
            .AddHostedServices()
            .AddMessageQueues();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IVolunteersRepository, VolunteersRepository>();
        services.AddScoped<ISpeciesRepository, SpeciesRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(
        this IServiceCollection services)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        return services;
    }
    
    private static IServiceCollection AddMessageQueues(
        this IServiceCollection services)
    {
        services.AddSingleton<IMessageQueue<IEnumerable<FileInfo>>, InMemoryMessageQueue<IEnumerable<FileInfo>>>();

        return services;
    }
    
    private static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddScoped<IFileCleanerService, FileCleanerService>();
        
        return services;
    }
    
    private static IServiceCollection AddHostedServices(
        this IServiceCollection services)
    {
        services.AddHostedService<FileCleanerBackgroundService>();
        
        return services;
    }


    private static IServiceCollection AddMinio(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SECTION_NAME));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.SECTION_NAME).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);
            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSSL);
        });
        
        services.AddScoped<IFileProvider, MinioProvider>();
        
        return services;
    }
}