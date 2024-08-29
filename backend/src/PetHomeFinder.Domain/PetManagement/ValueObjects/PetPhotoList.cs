using System;
using PetHomeFinder.Domain.Pets;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

public record PetPhotoList
{
    public IReadOnlyList<PetPhoto> PetPhotos { get; }
    private PetPhotoList()
    {
    }

    private PetPhotoList(IEnumerable<PetPhoto> petPhotos)
    {
        PetPhotos = petPhotos.ToList();
    }

    public static PetPhotoList Create(IEnumerable<PetPhoto> petPhotos) => new PetPhotoList(petPhotos);

}
