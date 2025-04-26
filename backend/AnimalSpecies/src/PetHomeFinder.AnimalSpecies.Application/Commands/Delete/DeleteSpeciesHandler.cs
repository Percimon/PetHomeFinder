using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.Volunteers.Contracts;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IPetsContract _petsContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        IValidator<DeleteSpeciesCommand> validator,
        ISpeciesRepository speciesRepository,
        IPetsContract petsContract,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _petsContract = petsContract;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = await _petsContract.AnyPetIsOfSpecies(command.SpeciesId, cancellationToken);
        if (speciesQuery.IsFailure)
        {
            return speciesQuery.Error; 
        }
        
        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            return speciesResult.Error.ToErrorList();
        }

        var deleteResult = _speciesRepository.Delete(speciesResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Species deleted with id: {speciesId}.", deleteResult);

        return deleteResult;
    }
}