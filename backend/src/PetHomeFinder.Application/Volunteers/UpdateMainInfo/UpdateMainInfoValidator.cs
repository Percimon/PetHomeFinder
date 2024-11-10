using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.UpdateMainInfo;

public class UpdateMainInfoValidator : AbstractValidator<UpdateMainInfoRequest>
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
