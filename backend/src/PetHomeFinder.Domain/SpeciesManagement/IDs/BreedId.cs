using System;

namespace PetHomeFinder.Domain.SpeciesManagement.IDs;

public record BreedId
{
    public Guid Value { get; }

    private BreedId(Guid value)
    {
        Value = value;
    }

    public static BreedId Create(Guid id) => new BreedId(id);
    public static BreedId New() => new BreedId(Guid.NewGuid());
    public static BreedId Empty() => new BreedId(Guid.Empty);
}
