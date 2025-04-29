using FluentValidation;
using PetHomeFinder.Core.Validation;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Delete;

public class DeleteSpeciesValidator : AbstractValidator<DeleteSpeciesCommand>
{
    public DeleteSpeciesValidator()
    {
        RuleFor(s => s.SpeciesId).NotEmpty().WithError(Errors.General.ValueIsRequired());
    }
}