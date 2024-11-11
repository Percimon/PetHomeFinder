using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Application.SpeciesBreeds.Create;

public class CreateSpeciesHandler
{
    private readonly IValidator<CreateSpeciesRequest> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        IValidator<CreateSpeciesRequest> validator,
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieId = SpeciesId.New();
        var name = Name.Create(request.Name).Value;

        var species = new Species(specieId, name);

        await _speciesRepository.Add(species, cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return species.Id.Value;
    }
    
}