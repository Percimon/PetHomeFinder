using System;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Volunteers;

public record Experience
{
    public int Value { get; }

    private Experience(int value)
    {
        Value = value;
    }

    public static Result<Experience> Create(int value)
    {
        if (value < Constants.MIN_EXPERIENCE_PARAMETER)
            return Errors.General.ValueIsInvalid("Experience");

        return new Experience(value);
    }
}
