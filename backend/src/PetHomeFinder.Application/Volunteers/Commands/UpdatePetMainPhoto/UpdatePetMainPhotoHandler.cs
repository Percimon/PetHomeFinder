using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetMainPhoto;

public class UpdatePetMainPhotoHandler : ICommandHandler<Guid, UpdatePetMainPhotoCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdatePetMainPhotoCommand> _validator;
    private readonly ILogger<UpdatePetMainPhotoHandler> _logger;

    public UpdatePetMainPhotoHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdatePetMainPhotoCommand> validator,
        ILogger<UpdatePetMainPhotoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetMainPhotoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var filePathResult = FilePath.Create(command.FilePath);
        if (filePathResult.IsFailure)
            return filePathResult.Error.ToErrorList();

        var petPhotoResult = PetPhoto.Create(filePathResult.Value.Path, true);
        if (petPhotoResult.IsFailure)
            return petPhotoResult.Error.ToErrorList();

        var updateResult = volunteerResult.Value.UpdatePetMainPhoto(petResult.Value, petPhotoResult.Value);
        if (updateResult.IsFailure)
            return updateResult.Error.ToErrorList();

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Main photo updated for pet with id {PetId}", command.PetId);

        return command.PetId;
    }
}