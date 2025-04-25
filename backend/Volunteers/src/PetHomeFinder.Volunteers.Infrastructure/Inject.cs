using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Files;
using PetHomeFinder.Core.Messaging;
using PetHomeFinder.Core.Providers;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Infrastructure.BackgroundServices;
using PetHomeFinder.Volunteers.Infrastructure.DbContexts;
using PetHomeFinder.Volunteers.Infrastructure.Files;
using PetHomeFinder.Volunteers.Infrastructure.MessageQueues;
using PetHomeFinder.Volunteers.Infrastructure.Options;
using PetHomeFinder.Volunteers.Infrastructure.Providers;
using FileInfo = PetHomeFinder.Core.Files.FileInfo;
using Minio;

namespace PetHomeFinder.Volunteers.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddVolunteersInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration)
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
        
        return services;
    }
    
    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
        services.AddScoped<IReadDbContext, ReadDbContext>(_ =>
            new ReadDbContext(configuration.GetConnectionString(Constants.DATABASE)!));
        
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