using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Commands.HardDeletePetById;

public class HardDeletePetByIdCommandValidator : AbstractValidator<HardDeletePetByIdCommand>
{
    public HardDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}