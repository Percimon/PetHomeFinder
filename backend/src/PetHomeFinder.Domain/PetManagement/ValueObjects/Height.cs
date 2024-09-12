using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record Height
{
    public double Value { get; }

    private Height(double value)
    {
        Value = value;
    }

    public static Result<Height> Create(double value)
    {
        if (value < Constants.MIN_PHYSICAL_PARAMETER)
            return Errors.General.ValueIsInvalid("Height");

        return new Height(value);
    }

}
