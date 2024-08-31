using System;

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
}
