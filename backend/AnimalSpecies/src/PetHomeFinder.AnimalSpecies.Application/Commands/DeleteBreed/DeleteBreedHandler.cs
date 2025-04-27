using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.Volunteers.Contracts;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IPetsContract _petsContract;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        IValidator<DeleteBreedCommand> validator,
        ISpeciesRepository speciesRepository,
        IPetsContract petsContract,
        ILogger<DeleteBreedHandler> logger, 
        [FromKeyedServices(ModuleKey.AnimalSpecies)] IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _petsContract = petsContract;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var speciesQuery = await _petsContract.AnyPetIsOfSpecies(command.SpeciesId, cancellationToken);
        if (speciesQuery.IsFailure)
        {
            return speciesQuery.Error; 
        }
        
        var breedQuery = await _petsContract.AnyPetIsOfSpecies(command.BreedId, cancellationToken);
        if (breedQuery.IsFailure)
        {
            return breedQuery.Error; 
        }
        
        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);

        if (speciesResult.IsFailure)
        {
            return speciesResult.Error.ToErrorList();
        }
        
        var breed = speciesResult.Value.Breeds.FirstOrDefault(b => b.Id.Value == command.BreedId);
        if (breed is null)
        {
            return Errors.General.NotFound(command.BreedId).ToErrorList();
        }
        
        var deleteResult = speciesResult.Value.DeleteBreed(breed);
        
        _speciesRepository.Save(speciesResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Breed deleted with id: {breedId}.", deleteResult.Value);

        return deleteResult.Value;
    }
}