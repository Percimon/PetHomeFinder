using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.DeleteBreed;

public class DeleteBreedValidator : AbstractValidator<DeleteBreedCommand>
{
    public DeleteBreedValidator()
    {
        RuleFor(s => s.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
        RuleFor(s => s.BreedId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}