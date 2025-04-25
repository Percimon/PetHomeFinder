using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.Create;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(r => FullName.Create(r.FirstName, r.LastName, r.Surname));
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Link));
        RuleForEach(c => c.Credentials)
            .MustBeValueObject(r => Credential.Create(r.Name, r.Description));
    }
}