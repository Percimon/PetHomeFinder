using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets;

public record HealthInfo
{
    public string Value { get; }

    private HealthInfo(string value)
    {
        Value = value;
    }

    public static Result<HealthInfo> Create(string value)
    {
        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("HealthInfo");

        return new HealthInfo(value);
    }
}
