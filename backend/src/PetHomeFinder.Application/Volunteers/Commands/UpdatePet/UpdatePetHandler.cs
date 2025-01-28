using CSharpFunctionalExtensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePet;

public class UpdatePetHandler : ICommandHandler<Guid, UpdatePetCommand>
{
    private readonly ILogger<UpdatePetHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;

    public UpdatePetHandler(
        ILogger<UpdatePetHandler> logger,
        IUnitOfWork unitOfWork,
        IVolunteersRepository volunteersRepository,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _volunteersRepository = volunteersRepository;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var petResult = volunteerResult.Value.PetsOwning.FirstOrDefault(p => p.Id.Value == command.PetId);
        if (petResult is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var speciesResult = await _speciesRepository.GetById(command.SpeciesId, cancellationToken);
        if (speciesResult.IsFailure)
            return speciesResult.Error.ToErrorList();

        var breedResult = speciesResult.Value.Breeds.FirstOrDefault(b => b.Id.Value == command.BreedId);
        if (breedResult is null)
            return Errors.General.NotFound(command.BreedId).ToErrorList();

        var petId = PetId.Create(command.PetId);
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