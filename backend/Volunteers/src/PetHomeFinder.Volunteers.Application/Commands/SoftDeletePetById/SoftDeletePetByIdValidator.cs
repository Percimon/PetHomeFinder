using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Commands.SoftDeletePetById;

public class SoftDeletePetByIdCommandValidator : AbstractValidator<SoftDeletePetByIdCommand>
{
    public SoftDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}