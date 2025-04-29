using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Providers;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using FileInfo = PetHomeFinder.Core.Files.FileInfo;

namespace PetHomeFinder.Volunteers.Application.Commands.HardDeletePetById;

public class HardDeletePetByIdHandler : ICommandHandler<Guid, HardDeletePetByIdCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IFileProvider _fileProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<HardDeletePetByIdCommand> _validator;
    private readonly ILogger<HardDeletePetByIdHandler> _logger;

    public HardDeletePetByIdHandler(
        IVolunteersRepository volunteersRepository,
        IFileProvider fileProvider,
        [FromKeyedServices(ModuleKey.Volunteer)]IUnitOfWork unitOfWork,
        IValidator<HardDeletePetByIdCommand> validator,
        ILogger<HardDeletePetByIdHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _fileProvider = fileProvider;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        HardDeletePetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        var petFiles = petResult.Value.Photos
            .Select(p => 
                new FileInfo(FilePath.Create(p.FilePath).Value, Constants.BUCKET_NAME_PHOTOS))
            .ToList();

        volunteerResult.Value.HardDeletePet(petResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        foreach (var fileInfo in petFiles)
        {
            var deleteFileResult = await _fileProvider.DeleteFile(fileInfo, cancellationToken);
            if (deleteFileResult.IsFailure)
                return deleteFileResult.Error;
        }

        _logger.LogInformation("Pet was deleted with id: {PetId}.", petResult.Value.Id);

        return petResult.Value.Id.Value;
    }
}