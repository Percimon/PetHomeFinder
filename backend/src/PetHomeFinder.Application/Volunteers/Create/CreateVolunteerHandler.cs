using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Application.Volunteers.Create;

public class CreateVolunteerHandler
{
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IVolunteersRepository _repository;
    private readonly IValidator<CreateVolunteerRequest> _validator;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        IValidator<CreateVolunteerRequest> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
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

        _logger.LogInformation("Volunteer created with id: {VolunteerId}.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}
