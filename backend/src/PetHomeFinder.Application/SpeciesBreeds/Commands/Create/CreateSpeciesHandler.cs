using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.Create;

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
        IUnitOfWork unitOfWork)
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
        if (result.IsSuccess)
            return result.Error.ToErrorList();
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Species added with id {specieId}.", specieId.Value);

        return species.Id.Value;
    }
    
}