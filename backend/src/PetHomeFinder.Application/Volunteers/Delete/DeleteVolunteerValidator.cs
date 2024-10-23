using System;
using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Delete;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}
