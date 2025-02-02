using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.HardDeletePetById;

public class HardDeletePetByIdCommandValidator : AbstractValidator<HardDeletePetByIdCommand>
{
    public HardDeletePetByIdCommandValidator()
    {
        RuleFor(s => s.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}