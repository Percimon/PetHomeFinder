using FluentValidation;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.Core.Validation;

namespace PetHomeFinder.AnimalSpecies.Application.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}