using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Domain.Entities;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Create;

public class CreateSpeciesHandler : ICommandHandler<Guid, CreateSpeciesCommand>
{
    private readonly IValidator<CreateSpeciesCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSpeciesHandler> _logger;

    public CreateSpeciesHandler(
        IValidator<CreateSpeciesCommand> validator,
        ISpeciesRepository speciesRepository,
        ILogger<CreateSpeciesHandler> logger, 
        [FromKeyedServices(ModuleKey.AnimalSpecies)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<Guid, ErrorList>> Handle(
        CreateSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var specieId = SpeciesId.New();
        
        var name = Name.Create(command.Name).Value;

        var species = new Species(specieId, name);

        var result = await _speciesRepository.Add(species, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return species.Id.Value;
    }
    
}