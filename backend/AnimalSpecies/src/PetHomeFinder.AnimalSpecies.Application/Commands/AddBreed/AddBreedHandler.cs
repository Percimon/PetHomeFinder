using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.AnimalSpecies.Domain.Entities;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;

public class AddBreedHandler : ICommandHandler<Guid, AddBreedCommand>
{
    private readonly IValidator<AddBreedCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddBreedHandler> _logger;

    public AddBreedHandler(
        IValidator<AddBreedCommand> validator,
        ISpeciesRepository speciesRepository,
        ILogger<AddBreedHandler> logger, 
        [FromKeyedServices(ModuleKey.AnimalSpecies)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        AddBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if(speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();
        
        var breedId = BreedId.New();
        
        var nameResult = Name.Create(command.Name);
        if(nameResult.IsFailure)
            return nameResult.Error.ToErrorList();
        
        var breed = new Breed(breedId, nameResult.Value);

        var addBreedResult = speciesResult.Value.AddBreed(breed);
        if (addBreedResult.IsFailure)
            return addBreedResult.Error.ToErrorList();
        
        _speciesRepository.Save(speciesResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Breed added with id {breedId}.", breedId.Value);

        return breed.Id.Value;
    }
}