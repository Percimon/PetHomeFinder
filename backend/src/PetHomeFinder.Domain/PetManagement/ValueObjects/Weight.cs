using System;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Weight
{
    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    public static Weight Create(double value)
    {
        return new Weight(value);
    }

}
