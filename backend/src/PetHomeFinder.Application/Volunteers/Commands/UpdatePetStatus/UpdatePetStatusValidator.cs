using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetStatus;

public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Status).Must(s => Constants.PERMITTED_PET_STATUS_FOR_VOLUNTEER.Contains(s));
    }
}