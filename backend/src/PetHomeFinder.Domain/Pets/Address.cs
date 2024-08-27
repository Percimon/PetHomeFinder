using System;

namespace PetHomeFinder.Domain.Pets;

public record Address
{
    public string Value { get; }

    private Address(string value)
    {
        Value = value;
    }

    public static Address Create(string value) => new Address(value);
}
