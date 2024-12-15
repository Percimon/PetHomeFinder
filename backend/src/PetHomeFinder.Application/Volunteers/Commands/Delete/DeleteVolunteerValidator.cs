using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerCommand>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
