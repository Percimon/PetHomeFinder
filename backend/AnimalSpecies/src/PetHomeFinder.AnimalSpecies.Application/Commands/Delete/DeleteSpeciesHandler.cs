using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IPetContract _readDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        IValidator<DeleteSpeciesCommand> validator,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        IUnitOfWork unitOfWork,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        var petsQuery = _readDbContext.Pets;

        var result = petsQuery.FirstOrDefault(pet => pet.SpeciesId == command.SpeciesId);
        if (result != null)
        {
            return Errors.General.IsUsed(nameof(Species), command.SpeciesId).ToErrorList();
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