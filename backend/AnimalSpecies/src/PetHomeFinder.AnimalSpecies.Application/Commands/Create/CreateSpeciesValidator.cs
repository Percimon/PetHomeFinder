using FluentValidation;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.Core.Validation;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.Create;

public class CreateSpeciesValidator : AbstractValidator<CreateSpeciesCommand>
{
    public CreateSpeciesValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}