using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.Entities;

namespace PetHomeFinder.Volunteers.Application.Commands.Create;

public class CreateVolunteerHandler : ICommandHandler<Guid, CreateVolunteerCommand>
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVolunteerHandler> _logger;
    private readonly IValidator<CreateVolunteerCommand> _validator;

    public CreateVolunteerHandler(
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork,
        IValidator<CreateVolunteerCommand> validator,
        ILogger<CreateVolunteerHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        CreateVolunteerCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
        {
            return validationResult.ToErrorList();
        }

        var id = VolunteerId.New();

        var fullNameDto = command.FullName;
        var descriptionDto = command.Description;
        var experienceDto = command.Experience;
        var phoneNumberDto = command.PhoneNumber;
        var credentialListDto = command.Credentials;
        var socialNetworkListDto = command.SocialNetworks;

        var fullName = FullName.Create(fullNameDto.FirstName, fullNameDto.LastName, fullNameDto.Surname);
        var description = Description.Create(descriptionDto);
        var experience = Experience.Create(experienceDto);
        var phoneNumber = PhoneNumber.Create(phoneNumberDto);

        var credentialList = credentialListDto
            .Select(c => Credential.Create(c.Name, c.Description).Value);

        var socialNetworkList = socialNetworkListDto.
            Select(c => SocialNetwork.Create(c.Name, c.Link).Value);

        var volunteer = new Volunteer(
            id,
            fullName.Value,
            description.Value,
            experience.Value,
            phoneNumber.Value,
            credentialList,
            socialNetworkList
        );

        await _repository.Add(volunteer, cancellationToken);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Volunteer created with id: {VolunteerId}.", volunteer.Id.Value);

        return volunteer.Id.Value;
    }
}