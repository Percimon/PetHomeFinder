using System;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets;

public record Color
{
    public string Value { get; }

    private Color(string value)
    {
        Value = value;
    }

    public static Result<Color> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("color");

        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("color");

        return new Color(value);
    }
}
