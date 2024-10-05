using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Experience
{
    public int Value { get; }

    private Experience(int value)
    {
        Value = value;
    }

    public static Result<Experience, Error> Create(int value)
    {
        if (value < Constants.MIN_EXPERIENCE_PARAMETER)
            return Errors.General.ValueIsInvalid("Experience");

        return new Experience(value);
    }
}
