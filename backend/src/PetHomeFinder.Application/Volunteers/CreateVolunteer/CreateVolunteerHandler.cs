using System;
using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    public async Task<Result<Guid>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var id = VolunteerId.New();

        var fullNameDto = request.FullName;
        var descriptionDto = request.Description;
        var experienceDto = request.Experience;
        var phoneNumberDto = request.PhoneNumber;
        var credentialListDto = request.CredentialList;
        var socialNetworkListDto = request.SocialNetworkList;

        var fullName = FullName.Create(fullNameDto.FirstName, fullNameDto.LastName, fullNameDto.Surname);
        var description = Description.Create(descriptionDto);
        var experience = Experience.Create(experienceDto);
        var phoneNumber = PhoneNumber.Create(phoneNumberDto);

        var credentialList = CredentialList.Create(
            credentialListDto.Credentials
                                .Select(c => Credential.Create(c.Name, c.Description).Value)
                                );

        var socialNetworkList = SocialNetworkList.Create(
            socialNetworkListDto.SocialNetworks
                    .Select(c => SocialNetwork.Create(c.Name, c.Link))
                    );

        var volunteer = new Volunteer(
            id,
            fullName,
            description,
            experience,
            phoneNumber,
            credentialList,
            socialNetworkList
        );

        return volunteer.Id.Value;
    }
}
