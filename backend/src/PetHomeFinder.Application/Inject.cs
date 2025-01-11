using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.SpeciesBreeds.AddBreed;
using PetHomeFinder.Application.SpeciesBreeds.Create;
using PetHomeFinder.Application.Volunteers.Commands.AddPet;
using PetHomeFinder.Application.Volunteers.Commands.Create;
using PetHomeFinder.Application.Volunteers.Commands.Delete;
using PetHomeFinder.Application.Volunteers.Commands.FileTest.Delete;
using PetHomeFinder.Application.Volunteers.Commands.FileTest.Get;
using PetHomeFinder.Application.Volunteers.Commands.FileTest.Upload;
using PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;
using PetHomeFinder.Application.Volunteers.Commands.UpdateMainInfo;
using PetHomeFinder.Application.Volunteers.Commands.UpdateSocialNetworks;
using PetHomeFinder.Application.Volunteers.Commands.UploadFilesToPet;

namespace PetHomeFinder.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommands().AddQueries();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }
 
    private static IServiceCollection AddCommands(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
    
    private static IServiceCollection AddQueries(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssemblies(typeof(Inject).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}
