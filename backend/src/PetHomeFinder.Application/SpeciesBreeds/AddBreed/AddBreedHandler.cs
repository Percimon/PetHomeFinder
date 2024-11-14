using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.SpeciesBreeds.Create;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.Entities;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Application.SpeciesBreeds.AddBreed;

public class AddBreedHandler
{
    private readonly IValidator<AddBreedRequest> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<AddBreedHandler> _logger;

    public AddBreedHandler(
        IValidator<AddBreedRequest> validator,
        ISpeciesRepository speciesRepository,
        ILogger<AddBreedHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesResult = await _speciesRepository.GetById(request.SpeciesId, cancellationToken);
        if(speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        var breedId = BreedId.New();
        
        var nameResult = Name.Create(request.Name);
        if(nameResult.IsFailure)
            return nameResult.Error.ToErrorList();
        
        var breed = new Breed(breedId, nameResult.Value);

        var addBreedResult = speciesResult.Value.AddBreed(breed);
        if (addBreedResult.IsFailure)
            return addBreedResult.Error.ToErrorList();
        
        await _speciesRepository.Save(speciesResult.Value, cancellationToken);
        
        _logger.LogInformation("Breed added with id {breedId}.", breedId.Value);

        return breed.Id.Value;
    }
}