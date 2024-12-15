using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdateMainInfo;

public class UpdateMainInfoHandler
{
    private readonly ILogger<UpdateMainInfoHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateMainInfoCommand> _validator;

    public UpdateMainInfoHandler(
        IVolunteersRepository volunteersRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateMainInfoCommand> validator,
        ILogger<UpdateMainInfoHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateMainInfoCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var fullName = FullName.Create(
            command.FullName.FirstName,
            command.FullName.LastName,
            command.FullName.Surname)
            .Value;
        var description = Description.Create(command.Description).Value;
        var experience = Experience.Create(command.Experience).Value;
        var phoneNumber = PhoneNumber.Create(command.PhoneNumber).Value;

        volunteerResult.Value.UpdateMainInfo(
            fullName,
            description,
            experience,
            phoneNumber);

        _volunteersRepository.Save(volunteerResult.Value);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Main info of volunteer updated with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}
