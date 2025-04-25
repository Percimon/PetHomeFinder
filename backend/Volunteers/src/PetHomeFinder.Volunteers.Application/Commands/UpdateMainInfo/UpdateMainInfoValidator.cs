using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdateMainInfo;

public class UpdateMainInfoValidator : AbstractValidator<UpdateMainInfoCommand>
{
    public UpdateMainInfoValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());

        RuleFor(c => c.FullName)
            .MustBeValueObject(r => FullName.Create(r.FirstName, r.LastName, r.Surname));
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}
