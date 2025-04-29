using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record UpdatePetRequest(
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
    IEnumerable<CredentialDto> Credentials);