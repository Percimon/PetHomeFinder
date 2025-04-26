using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Contracts;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.Entities;
using PetHomeFinder.Volunteers.Domain.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.AddPet;

public class AddPetHandler : ICommandHandler<Guid, AddPetCommand>
{
    private readonly ILogger<AddPetHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnimalSpeciesContract _animalSpeciesContract;
    private readonly IAnimalSpeciesContract _speciesContract;
    private readonly IValidator<AddPetCommand> _validator;

    public AddPetHandler(
        ILogger<AddPetHandler> logger,
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IAnimalSpeciesContract animalSpeciesContract,
        IValidator<AddPetCommand> validator)
    {
        _logger = logger;
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _animalSpeciesContract = animalSpeciesContract;
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

        var speciesQuery = await _animalSpeciesContract.SpeciesExists(
            command.SpeciesId, 
            cancellationToken);
        if (speciesQuery.IsFailure)
            return speciesQuery.Error;

        var breedQuery = await _animalSpeciesContract.BreedExists(
            command.SpeciesId, 
            command.BreedId, 
            cancellationToken);
        if (breedQuery.IsFailure)
            return breedQuery.Error;

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
        var credentials = command.Credentials
            .Select(c => Credential.Create(c.Name, c.Description).Value);

        var helpStatus = Enum.Parse<HelpStatusEnum>(command.HelpStatus);
        
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
            helpStatus,
            credentials,
            command.CreateDate);

        volunteerResult.Value.AddPet(pet);

        _volunteersRepository.Save(volunteerResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet added with id: {PetId}.", pet.Id.Value);

        return pet.Id.Value;
    }
}