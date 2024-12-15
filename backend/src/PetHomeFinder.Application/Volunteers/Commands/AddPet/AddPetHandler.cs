using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Application.Volunteers.Commands.AddPet;

public class AddPetHandler
{
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        ISpeciesRepository speciesRepository,
        IValidator<AddPetCommand> validator)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _speciesRepository = speciesRepository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        AddPetCommand command,
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

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
        {
            return speciesResult.Error.ToErrorList();
        }

        var breedResult = speciesResult.Value
            .Breeds
            .FirstOrDefault(b => b.Id.Value == command.BreedId);
        if (breedResult == null)
        {
            return Errors.General.NotFound(command.BreedId).ToErrorList();
        }

        var petId = PetId.New();
        var name = Name.Create(command.Name).Value;
        var speciesBreed = SpeciesBreed.Create(
                SpeciesId.Create(command.SpeciesId),
                BreedId.Create(command.BreedId).Value)
            .Value;
        var description = Description.Create(command.Description).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var address = Address.Create(
            command.Address.City,
            command.Address.District,
            command.Address.Street,
            command.Address.Street).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.OwnerPhoneNumber).Value;
        var credentials = new CredentialList(
            command.Credentials.Credentials.Select(c =>
                Credential.Create(c.Name, c.Description).Value));

        var pet = new Pet(petId,
            name,
            speciesBreed,
            description,
            color,
            healthInfo,
            address,
            weight,
            height,
            phoneNumber,
            command.IsCastrated,
            command.IsVaccinated,
            command.BirthDate,
            command.HelpStatus,
            credentials,
            command.CreateDate);

        volunteerResult.Value.AddPet(pet);
        
        _volunteersRepository.Save(volunteerResult.Value);
        
        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet added with id: {PetId}.", pet.Id.Value);

        return pet.Id.Value;
    }
}