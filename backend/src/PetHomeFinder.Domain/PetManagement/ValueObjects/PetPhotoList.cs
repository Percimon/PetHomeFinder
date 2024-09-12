using System;
using PetHomeFinder.Domain.Pets;
using PetHomeFinder.Domain.Shared;

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

    public static Result<PetPhotoList> Create(IEnumerable<PetPhoto> petPhotos)
    {
        return new PetPhotoList(petPhotos);
    }
}
