using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.AddPet;
using PetHomeFinder.Domain.PetManagement.Entities;

namespace PetHomeFinder.API.Controllers.Volunteers.Requests;

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
    CredentialListDto Credentials,
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