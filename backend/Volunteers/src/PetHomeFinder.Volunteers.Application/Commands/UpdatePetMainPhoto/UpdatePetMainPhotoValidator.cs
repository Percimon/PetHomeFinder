using FluentValidation;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Commands.UpdatePetMainPhoto;

public class UpdatePetMainPhotoValidator : AbstractValidator<UpdatePetMainPhotoCommand>
{
    public UpdatePetMainPhotoValidator()
    {
        RuleFor(u => u.VolunteerId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.PetId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(u => u.FilePath).MustBeValueObject(FilePath.Create);
    }
}