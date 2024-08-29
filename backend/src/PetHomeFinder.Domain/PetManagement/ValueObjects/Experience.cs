using System;

namespace PetHomeFinder.Domain.Volunteers;

public record Experience
{
    public int Value { get; }

    private Experience(int value)
    {
        Value = value;
    }

    public static Experience Create(int value) => new Experience(value);
}
