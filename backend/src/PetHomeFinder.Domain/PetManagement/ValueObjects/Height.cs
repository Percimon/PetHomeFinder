using System;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Height
{
    public double Value { get; }

    private Height(double value)
    {
        Value = value;
    }

    public static Height Create(double value)
    {
        return new Height(value);
    }

}
