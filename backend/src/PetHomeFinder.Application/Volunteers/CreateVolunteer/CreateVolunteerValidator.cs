using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerValidator()
    {
        RuleFor(c => c.FullName)
            .MustBeValueObject(r => FullName.Create(r.FirstName, r.LastName, r.Surname));
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.SocialNetworkList.SocialNetworks)
            .MustBeValueObject(r => SocialNetwork.Create(r.Name, r.Link));
        RuleForEach(c => c.CredentialList.Credentials)
            .MustBeValueObject(r => Credential.Create(r.Name, r.Description));
    }
}