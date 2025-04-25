using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Domain.Entities;

namespace PetHomeFinder.Volunteers.Application.Commands.AddPet;

public record AddPetCommand(
    Guid VolunteerId,
    string Name,
    Guid SpeciesId,
    Guid BreedId,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string OwnerPhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    DateTime BirthDate,
    HelpStatusEnum HelpStatus,
    IEnumerable<CredentialDto> Credentials,
    DateTime CreateDate) : ICommand;