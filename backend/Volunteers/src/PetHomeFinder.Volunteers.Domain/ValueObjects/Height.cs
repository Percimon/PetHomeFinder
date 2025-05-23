using CSharpFunctionalExtensions;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Domain.ValueObjects;

public record Height
{
    public double Value { get; }

    private Height(double value)
    {
        Value = value;
    }

    public static Result<Height, Error> Create(double value)
    {
        if (value < Constants.MIN_PHYSICAL_PARAMETER)
            return Errors.General.ValueIsInvalid("Height");

        return new Height(value);
    }

}
