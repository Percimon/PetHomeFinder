using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(s => s.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}