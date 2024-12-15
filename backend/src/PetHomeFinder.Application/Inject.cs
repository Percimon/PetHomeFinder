using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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
        services.AddHandlers();
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateVolunteerHandler>();
        
        services.AddScoped<UpdateMainInfoHandler>();
        services.AddScoped<UpdateCredentialsHandler>();
        services.AddScoped<UpdateSocialNetworksHandler>();
        services.AddScoped<AddPetHandler>();
        
        services.AddScoped<DeleteVolunteerHandler>();
        
        services.AddScoped<CreateSpeciesHandler>();
        services.AddScoped<AddBreedHandler>();
        
        services.AddScoped<UploadFileHandler>();
        services.AddScoped<UploadFilesToPetHandler>();
        services.AddScoped<DeleteFileHandler>();
        services.AddScoped<GetFileHandler>();
        
        return services;
    }
}
