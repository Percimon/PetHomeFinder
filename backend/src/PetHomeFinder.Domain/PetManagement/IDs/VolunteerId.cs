namespace PetHomeFinder.Domain.PetManagement.IDs;

public record VolunteerId
{
    public Guid Value { get; }

    private VolunteerId(Guid value)
    {
        Value = value;
    }

    public static VolunteerId Create(Guid id) => new VolunteerId(id);
    public static VolunteerId New() => new VolunteerId(Guid.NewGuid());
    public static VolunteerId Empty() => new VolunteerId(Guid.Empty);
    public static implicit operator VolunteerId(Guid id) => new(id);
    public static implicit operator Guid(VolunteerId volunteerId)
    {
        ArgumentNullException.ThrowIfNull(volunteerId);

        return volunteerId.Value;
    }
}
