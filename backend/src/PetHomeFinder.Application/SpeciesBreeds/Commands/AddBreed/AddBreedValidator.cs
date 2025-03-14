using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedCommand>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}