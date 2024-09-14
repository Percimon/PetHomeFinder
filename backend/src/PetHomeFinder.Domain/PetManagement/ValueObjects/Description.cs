using System;

namespace PetHomeFinder.Domain.Shared;

public record Description
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description> Create(string value)
    {
        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Description");

        return new Description(value);
    }
}
