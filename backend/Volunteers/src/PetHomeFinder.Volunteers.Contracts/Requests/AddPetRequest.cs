using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record AddPetRequest(
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
    string HelpStatus,
    IEnumerable<CredentialDto> Credentials,
    DateTime CreateDate);