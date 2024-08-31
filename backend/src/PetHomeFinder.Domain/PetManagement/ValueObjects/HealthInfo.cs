using System;

namespace PetHomeFinder.Domain.Pets;

public record HealthInfo
{
    public string Value { get; }

    private HealthInfo(string value)
    {
        Value = value;
    }

    public static HealthInfo Create(string value) => new HealthInfo(value);
}
