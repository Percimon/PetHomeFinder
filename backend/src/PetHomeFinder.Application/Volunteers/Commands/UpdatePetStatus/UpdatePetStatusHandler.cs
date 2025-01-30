using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetStatus;

public class UpdatePetStatusHandler : ICommandHandler<Guid, UpdatePetStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePetStatusHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly UpdatePetStatusValidator _validator;

    public UpdatePetStatusHandler(
        UpdatePetStatusValidator validator,
        IUnitOfWork unitOfWork,
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

        var volunteerResult = await _volunteersRepository
            .GetById(VolunteerId.Create(command.VolunteerId), cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();
        
        var petExistResult = volunteerResult.Value
            .PetsOwning.FirstOrDefault(p => p.Id.Value == command.PetId);
        if(petExistResult is null)
            return Errors.General.NotFound(command.PetId).ToErrorList();

        var status = Enum.Parse<HelpStatusEnum>(command.Status);

        var petId = PetId.Create(command.PetId);
        
        volunteerResult.Value.UpdatePetStatus(petId, status);

        await _unitOfWork.SaveChanges(cancellationToken);
        
        _logger.LogInformation("Pet with id = {PetId} status updated", command.PetId);
        
        return command.PetId;
    }
}