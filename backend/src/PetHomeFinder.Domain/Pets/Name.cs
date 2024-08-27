using System;

namespace PetHomeFinder.Domain.Pets;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Name Create(string value) => new Name(value);
}
