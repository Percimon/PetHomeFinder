using System;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets;

public record Name
{
    public string Value { get; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired("Name");

        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Name");

        return new Name(value);
    }
}
