using FluentValidation;
using PetHomeFinder.Application.Validation;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.AddBreed;

public class AddBreedValidator : AbstractValidator<AddBreedRequest>
{
    public AddBreedValidator()
    {
        RuleFor(c => c.Name).MustBeValueObject(Name.Create);
    }
}