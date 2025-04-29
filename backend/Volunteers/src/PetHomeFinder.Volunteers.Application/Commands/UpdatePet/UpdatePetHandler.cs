using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Contracts;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IAnimalSpeciesContract _animalSpeciesContract;
    
    public UpdatePetHandler(
        ILogger<UpdatePetHandler> logger,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        IAnimalSpeciesContract animalSpeciesContract)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _animalSpeciesContract = animalSpeciesContract;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petId = PetId.Create(command.PetId);

        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

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

        var name = Name.Create(command.Name).Value;
        var description = Description.Create(command.Description).Value;
        var address = Address.Create(
            command.Address.City,
            command.Address.District,
            command.Address.Street,
            command.Address.Structure).Value;
        var color = Color.Create(command.Color).Value;
        var healthInfo = HealthInfo.Create(command.HealthInfo).Value;
        var weight = Weight.Create(command.Weight).Value;
        var height = Height.Create(command.Height).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;
        var credentials = command.Credentials
            .Select(c => Credential.Create(c.Name, c.Description).Value);
        var speciesBreed = SpeciesBreed.Create(command.SpeciesId, command.BreedId).Value;

        volunteerResult.Value.UpdatePet(petId,
            name,
            description,
            speciesBreed,
            color,
            healthInfo,
            address,
            weight,
            height,
            phoneNumber,
            command.IsCastrated,
            command.IsVaccinated,
            command.BirthDate,
            credentials);

        await _unitOfWork.SaveChanges(cancellationToken);

        return command.PetId;
    }
}