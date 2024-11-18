using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.FileProvider;
using PetHomeFinder.Application.Providers;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.UploadFilesToPet;

public class UploadFilesToPetHandler
{
    private const string BUCKET_NAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly ILogger<UploadFilesToPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<UploadFilesToPetCommand> _validator;

    public UploadFilesToPetHandler(
        ILogger<UploadFilesToPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        IValidator<UploadFilesToPetCommand> validator, 
        IFileProvider fileProvider)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _validator = validator;
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

        //var transaction = _unitOfWork.BeginTransaction(cancellationToken);
        
        List<FileData> filesData = [];
        foreach (var file in command.Files)
        {
            var extension = Path.GetExtension(file.FileName);

            var filePath = FilePath.Create(Guid.NewGuid(), extension);
            if (filePath.IsFailure)
                return filePath.Error.ToErrorList();
            
            var fileData = new FileData(file.Content, filePath.Value, BUCKET_NAME);

            filesData.Add(fileData);
        }

        var filePathsResult = await _fileProvider.UploadFiles(filesData, cancellationToken);
        if (filePathsResult.IsFailure)
        {
            return filePathsResult.Error.ToErrorList();
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