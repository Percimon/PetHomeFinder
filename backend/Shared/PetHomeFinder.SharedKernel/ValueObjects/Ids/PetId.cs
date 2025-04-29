namespace PetHomeFinder.SharedKernel.ValueObjects.Ids;

public record PetId
{
    public Guid Value { get; }

    private PetId(Guid value)
    {
        Value = value;
    }

    public static PetId Create(Guid id) => new PetId(id);
    public static PetId New() => new PetId(Guid.NewGuid());
    public static PetId Empty() => new PetId(Guid.Empty);

}
