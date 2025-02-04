using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.PetManagement.Entities;

namespace PetHomeFinder.Application.Volunteers.Commands.AddPet;

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