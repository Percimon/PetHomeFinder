using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Messaging;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using FileInfo = PetHomeFinder.Application.FileProvider.FileInfo;

namespace PetHomeFinder.Application.Volunteers.Commands.UploadFilesToPet;

public class UploadFilesToPetHandler : ICommandHandler<Guid, UploadFilesToPetCommand>
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly ILogger<UploadFilesToPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<UploadFilesToPetCommand> _validator;
    private readonly IMessageQueue<IEnumerable<FileInfo>> _messageQueue;

    public UploadFilesToPetHandler(
        ILogger<UploadFilesToPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        IValidator<UploadFilesToPetCommand> validator, 
        IMessageQueue<IEnumerable<FileInfo>> messageQueue,
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
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

            var fileInfo = new FileInfo(filePath.Value, BUCKET_NAME);
            
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