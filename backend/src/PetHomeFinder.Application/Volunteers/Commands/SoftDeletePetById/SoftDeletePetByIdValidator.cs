using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.SoftDeletePetById;

public class SoftDeletePetByIdCommandValidator : AbstractValidator<SoftDeletePetByIdCommand>
{
    public SoftDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}