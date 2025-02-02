using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Commands.UpdatePetMainPhoto;

public class UpdatePetMainPhotoValidator : AbstractValidator<UpdatePetMainPhotoCommand>
{
    public UpdatePetMainPhotoValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.FilePath).MustBeValueObject(FilePath.Create);
    }
}