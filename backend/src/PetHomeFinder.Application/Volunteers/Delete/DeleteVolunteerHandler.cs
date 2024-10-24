using System;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Delete;

public class DeleteVolunteerHandler
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<DeleteVolunteerRequest> _validator;
    private readonly ILogger<DeleteVolunteerHandler> _logger;

    public DeleteVolunteerHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<DeleteVolunteerRequest> validator,
        ILogger<DeleteVolunteerHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteVolunteerRequest request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(request.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        volunteerResult.Value.SoftDelete();
        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Deleted volunteer with id: {VolunteerId}.", request.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}
