using FluentValidation;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;
using PetHomeFinder.Volunteers.Domain.ValueObjects;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePet;

public class UpdatePetValidator : AbstractValidator<UpdatePetCommand>
{
    public UpdatePetValidator()
    {
        RuleFor(c => c.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(c => c.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
        RuleFor(c => c.Description).MustBeValueObject(Description.Create);
        RuleFor(c => c.Color).MustBeValueObject(Color.Create);
        RuleFor(c => c.HealthInfo).MustBeValueObject(HealthInfo.Create);
        RuleFor(c => c.Address).MustBeValueObject(a => 
            Address.Create(a.City, a.District, a.Street, a.Structure));
        RuleFor(c => c.Weight).MustBeValueObject(Weight.Create);
        RuleFor(c => c.Height).MustBeValueObject(Height.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
        RuleForEach(c => c.Credentials).MustBeValueObject(c =>
            Credential.Create(c.Name, c.Description));
        
    }
}