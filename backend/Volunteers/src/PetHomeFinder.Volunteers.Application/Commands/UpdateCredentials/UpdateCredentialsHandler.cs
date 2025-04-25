using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdateCredentials;

public class UpdateCredentialsHandler : ICommandHandler<Guid, UpdateCredentialsCommand>
{
    private readonly ILogger<UpdateCredentialsHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateCredentialsCommand> _validator;

    public UpdateCredentialsHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateCredentialsCommand> validator,
        ILogger<UpdateCredentialsHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
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

        var credentials = new ValueObjectList<Credential>(command.Credentials
            .Select(r => Credential.Create(r.Name, r.Description).Value));

        volunteerResult.Value.UpdateCredentials(credentials);

        _volunteersRepository.Save(volunteerResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Credentials of volunteer updated with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}