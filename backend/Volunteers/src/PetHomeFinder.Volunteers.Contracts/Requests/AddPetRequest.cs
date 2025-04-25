using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.AddPet;
using PetHomeFinder.Volunteers.Domain.Entities;

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
    HelpStatusEnum HelpStatus,
    IEnumerable<CredentialDto> Credentials,
    DateTime CreateDate)
{
    public AddPetCommand ToCommand(Guid volunteerId) =>
        new(volunteerId, 
            Name, 
            SpeciesId, 
            BreedId, 
            Description,
            Color,
            HealthInfo,
            Address,
            Weight,
            Height,
            OwnerPhoneNumber,
            IsCastrated,
            IsVaccinated,
            BirthDate,
            HelpStatus,
            Credentials,
            CreateDate);
}