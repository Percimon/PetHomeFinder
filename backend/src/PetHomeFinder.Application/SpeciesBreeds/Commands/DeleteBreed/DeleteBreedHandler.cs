using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.Entities;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.DeleteBreed;

public class DeleteBreedHandler : ICommandHandler<Guid, DeleteBreedCommand>
{
    private readonly IValidator<DeleteBreedCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteBreedHandler> _logger;

    public DeleteBreedHandler(
        IValidator<DeleteBreedCommand> validator,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteBreedHandler> logger, 
        IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteBreedCommand command,
        CancellationToken cancellationToken = default)
    {
        var petsQuery = _readDbContext.Pets;

        var result = petsQuery.FirstOrDefault(pet => pet.BreedId == command.BreedId);
        if (result is not null)
        {
            return Errors.General.IsUsed(nameof(Breed), command.BreedId).ToErrorList();
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