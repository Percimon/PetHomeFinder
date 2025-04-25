using CSharpFunctionalExtensions;

namespace PetHomeFinder.SharedKernel.ValueObjects;

public record PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("PhoneNumber");

        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("PhoneNumber");

        return new PhoneNumber(value);
    }
}
