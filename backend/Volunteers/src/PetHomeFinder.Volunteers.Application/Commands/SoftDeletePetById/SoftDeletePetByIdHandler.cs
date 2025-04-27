using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.Volunteers.Application.Commands.SoftDeletePetById;

public class SoftDeletePetByIdHandler : ICommandHandler<Guid, SoftDeletePetByIdCommand>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<SoftDeletePetByIdCommand> _validator;
    private readonly ILogger<SoftDeletePetByIdHandler> _logger;

    public SoftDeletePetByIdHandler(
        IVolunteersRepository volunteersRepository,
        [FromKeyedServices(ModuleKey.Volunteer)] IUnitOfWork unitOfWork,
        IValidator<SoftDeletePetByIdCommand> validator,
        ILogger<SoftDeletePetByIdHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        SoftDeletePetByIdCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var petId = PetId.Create(command.PetId);
        var petResult = volunteerResult.Value.GetPetById(petId);
        if (petResult.IsFailure)
            return petResult.Error.ToErrorList();

        volunteerResult.Value.SoftDeletePet(petResult.Value.Id);

        await _unitOfWork.SaveChanges(cancellationToken);

        _logger.LogInformation("Pet was soft deleted with id: {PetId}.", petResult.Value.Id);

        return petResult.Value.Id.Value;
    }
}