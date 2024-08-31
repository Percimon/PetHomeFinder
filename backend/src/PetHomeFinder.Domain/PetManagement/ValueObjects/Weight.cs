using System;
using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Weight
{
    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    public static Result<Weight, string> Create(double value)
    {
        if (value < Constants.MIN_PHYSICAL_PARAMETER)
            return "Weight can't be less than zero";

        return new Weight(value);
    }

}
