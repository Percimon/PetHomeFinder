using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;
using PetHomeFinder.Volunteers.Domain.Entities;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly UpdatePetStatusValidator _validator;

    public UpdatePetStatusHandler(
        UpdatePetStatusValidator validator,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        ILogger<UpdatePetStatusHandler> logger,
        IVolunteersRepository volunteersRepository)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdatePetStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerId = VolunteerId.Create(command.VolunteerId);
        
        var volunteerResult = await _volunteersRepository
            .GetById(volunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petId = PetId.Create(command.PetId);
        
        var petExistResult = volunteerResult.Value
            .GetPetById(petId);
        if(petExistResult.IsFailure)
            return petExistResult.Error.ToErrorList();

        var status = Enum.Parse<HelpStatusEnum>(command.Status);
        
        volunteerResult.Value.UpdatePetStatus(petId, status);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet with id = {PetId} status updated", command.PetId);
        
        return command.PetId;
    }
}