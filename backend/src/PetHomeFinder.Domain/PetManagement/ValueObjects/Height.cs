using System;
using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Height
{
    public double Value { get; }

    private Height(double value)
    {
        Value = value;
    }

    public static Result<Height, string> Create(double value)
    {
        if (value < Constants.MIN_PHYSICAL_PARAMETER)
            return "Height can't be less than zero";

        return new Height(value);
    }

}
