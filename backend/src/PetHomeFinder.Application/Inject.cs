using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.SpeciesBreeds.AddBreed;
using PetHomeFinder.Application.SpeciesBreeds.Create;
using PetHomeFinder.Application.Volunteers.Create;
using PetHomeFinder.Application.Volunteers.Delete;
using PetHomeFinder.Application.Volunteers.FileTest.Delete;
using PetHomeFinder.Application.Volunteers.FileTest.Get;
using PetHomeFinder.Application.Volunteers.FileTest.Upload;
using PetHomeFinder.Application.Volunteers.UpdateCredentials;
using PetHomeFinder.Application.Volunteers.UpdateMainInfo;
using PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

namespace PetHomeFinder.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddVolunteerHandlers();
        services.AddSpeciesBreedsHandlers();
        services.AddFileTestHandlers();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddVolunteerHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateCredentialsHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<DeleteVolunteerHandler>();
        
        return services;
    }
    
    private static IServiceCollection AddSpeciesBreedsHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<AddBreedHandler>();
        
        return services;
    }

    private static IServiceCollection AddFileTestHandlers(this IServiceCollection services)
    {
        services.AddScoped<UploadFileHandler>();
        services.AddScoped<DeleteFileHandler>();
        services.AddScoped<GetFileHandler>();
        
        return services;
    }
    
}
