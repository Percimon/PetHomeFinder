using System;
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
    private readonly IValidator<UpdateCredentialsRequest> _validator;

    public UpdateCredentialsHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateCredentialsRequest> validator,
        ILogger<UpdateCredentialsHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateCredentialsRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var credentials = new ValueObjectList<Credential>(request
            .CredentialList
            .Credentials
            .Select(r => Credential.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdateCredentials(credentials);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        return volunteerResult.Value.Id.Value;
    }
}
