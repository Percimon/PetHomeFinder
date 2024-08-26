using System;

namespace PetHomeFinder.Domain.Volunteers;

public record PetStatistics
{
    private PetStatistics(int petHomeFoundCount, int petSearchForHomeCount, int petTreatmentCount)
    {
        PetHomeFoundCount = petHomeFoundCount;
        PetSearchForHomeCount = petSearchForHomeCount;
        PetTreatmentCount = petTreatmentCount;
    }

    public int PetHomeFoundCount { get; }
    public int PetSearchForHomeCount { get; }
    public int PetTreatmentCount { get; }

    public static PetStatistics Create(int petHomeFoundCount, int petSearchForHomeCount, int petTreatmentCount)
        => new PetStatistics(petHomeFoundCount, petSearchForHomeCount, petTreatmentCount);
}
