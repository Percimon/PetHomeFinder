namespace PetHomeFinder.Domain.SpeciesManagement.IDs;

public record SpeciesId
{
    public Guid Value { get; }

    private SpeciesId(Guid value)
    {
        Value = value;
    }

    public static SpeciesId Create(Guid id) => new SpeciesId(id);
    public static SpeciesId New() => new SpeciesId(Guid.NewGuid());
    public static SpeciesId Empty() => new SpeciesId(Guid.Empty);
    public static implicit operator SpeciesId(Guid id) => new(id);
    public static implicit operator Guid(SpeciesId speciesId)
    {
        ArgumentNullException.ThrowIfNull(speciesId);

        return speciesId.Value;
    }
}
