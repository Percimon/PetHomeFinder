using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.UpdateCredentials;

public class UpdateCredentialsHandler
{
    private readonly ILogger<UpdateCredentialsHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateCredentialsCommand> _validator;

    public UpdateCredentialsHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateCredentialsCommand> validator,
        ILogger<UpdateCredentialsHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateCredentialsCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var credentials = new ValueObjectList<Credential>(command
            .CredentialList
            .Credentials
            .Select(r => Credential.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdateCredentials(credentials);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Credentials of volunteer updated with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}
