using System;

namespace PetHomeFinder.Domain.Shared;

public record PhoneNumber
{
    public string Value { get; }
    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value) =>
        new PhoneNumber(value);

}
