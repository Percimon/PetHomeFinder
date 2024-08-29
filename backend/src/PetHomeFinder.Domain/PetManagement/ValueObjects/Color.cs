using System;

namespace PetHomeFinder.Domain.Pets;

public record Color
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Color Create(string value) => new Color(value);
}
