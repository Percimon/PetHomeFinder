using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePetStatus;

public class UpdatePetStatusValidator : AbstractValidator<UpdatePetStatusCommand>
{
    public UpdatePetStatusValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.Status).Must(s => Constants.PERMITTED_PET_STATUS_FOR_VOLUNTEER.Contains(s));
    }
}