using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Domain.ValueObjects;

public record Weight
{
    public double Value { get; }

    private Weight(double value)
    {
        Value = value;
    }

    public static Result<Weight, Error> Create(double value)
    {
        if (value < Constants.MIN_PHYSICAL_PARAMETER)
            return Errors.General.ValueIsInvalid("Weight");

        return new Weight(value);
    }

}
