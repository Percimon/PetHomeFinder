using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePet;

public record UpdatePetCommand(
    Guid VolunteerId, 
    Guid PetId,
    Guid SpeciesId,
    Guid BreedId,
    string Name,
    string Description,
    string Color,
    string HealthInfo,
    AddressDto Address,
    double Weight,
    double Height,
    string PhoneNumber,
    bool IsCastrated,
    bool IsVaccinated,
    DateTime BirthDate,
    IEnumerable<CredentialDto> Credentials) : ICommand;