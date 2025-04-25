using CSharpFunctionalExtensions;

namespace PetHomeFinder.SharedKernel.ValueObjects;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Description");

        return new Description(value);
    }
}
