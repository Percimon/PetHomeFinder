using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerHandler
{
    private readonly IVolunteerRepository _repository;
    private readonly IValidator<CreateVolunteerRequest> _validator;

    public CreateVolunteerHandler(
        IVolunteerRepository repository,
        IValidator<CreateVolunteerRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

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

        var credentialList = new CredentialList(
            credentialListDto.Credentials
                                .Select(c => Credential.Create(c.Name, c.Description).Value));

        var socialNetworkList = new SocialNetworkList(
            socialNetworkListDto.SocialNetworks
                    .Select(c => SocialNetwork.Create(c.Name, c.Link).Value));

        var volunteer = new Volunteer(
            id,
            fullName.Value,
            description.Value,
            experience.Value,
            phoneNumber.Value,
            credentialList,
            socialNetworkList
        );

        await _repository.Add(volunteer);

        return volunteer.Id.Value;
    }
}
