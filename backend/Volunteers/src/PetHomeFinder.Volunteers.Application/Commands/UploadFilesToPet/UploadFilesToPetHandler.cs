using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Contracts;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Files;
using PetHomeFinder.Core.Messaging;
using PetHomeFinder.Core.Providers;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.ValueObjects;
using FileInfo = PetHomeFinder.Core.Files.FileInfo;

namespace PetHomeFinder.Volunteers.Application.Commands.UploadFilesToPet;

public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
{
    private readonly IFileProvider _fileProvider;
    private readonly ILogger<UploadFilesToPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadFilesToPetHandler(
        ILogger<UploadFilesToPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IValidator<UploadFilesToPetCommand> validator, 
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _messageQueue = messageQueue;
        _fileProvider = fileProvider;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UploadFilesToPetCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
        {
            return volunteerResult.Error.ToErrorList();
        }

        var petResult = volunteerResult.Value.GetPetById(PetId.Create(command.PetId));
        if (petResult.IsFailure)
        {
            return petResult.Error.ToErrorList();
        }

        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();

            var fileInfo = new FileInfo(filePath.Value, Constants.BUCKET_NAME_PHOTOS);
            
            var fileData = new FileData(file.Content, fileInfo);

            filesData.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            await _messageQueue.WriteAsync(filesData.Select(f => f.FileInfo), cancellationToken);
            
            return filePathsResult.Error;
        }

        var petPhotos = filePathsResult.Value
            .Select(f => PetPhoto.Create(f.Path, false).Value)
            .ToList();

        petResult.Value.UpdatePhotos(petPhotos);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Uploaded photos to pet - {petId}", petResult.Value.Id.Value);

        return petResult.Value.Id.Value;
    }
}