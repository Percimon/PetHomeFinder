using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.UpdatePet;

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
    IEnumerable<CredentialDto> Credentials)
{
    public UpdatePetCommand ToCommand(Guid volunteerId, Guid petId) =>
        new(volunteerId,
            petId,
            SpeciesId,
            BreedId,
            Name,
            Description,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            PhoneNumber,
            IsCastrated,
            IsVaccinated,
            BirthDate,
            Credentials);
}