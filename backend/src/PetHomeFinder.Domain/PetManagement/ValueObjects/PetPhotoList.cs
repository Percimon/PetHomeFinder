namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record PetPhotoList
{
    public IReadOnlyList<PetPhoto> PetPhotos { get; }

    //EF core constructor
    private PetPhotoList() { }

    public PetPhotoList(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }
}
