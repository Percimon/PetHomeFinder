using System;

namespace PetHomeFinder.Domain.Shared;

public record PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static Result<PhoneNumber> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("PhoneNumber");

        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("PhoneNumber");

        return new PhoneNumber(value);
    }
}
