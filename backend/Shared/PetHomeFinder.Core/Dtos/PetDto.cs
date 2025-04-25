namespace PetHomeFinder.Core.Dtos;

public class PetDto
{
    public Guid Id { get; init; }
    
    public Guid VolunteerId { get; init; }
    
    public Guid SpeciesId { get; init; }

    public Guid BreedId { get; init; }

    public string  Name { get; init; }
    
    public string Description { get; init; }
   
    public string Color { get; init; }
    
    public string HealthInfo { get; init; }
    
    public AddressDto Address { get; init; }
    
    public double Height { get; init; }
    
    public double Weight { get; init; }
    
    public string OwnerPhoneNumber { get; init; }

    public bool IsCastrated { get; init; }
    
    public bool IsVaccinated { get; init; }
    
    public DateTime BirthDate { get; init; }
    
    public string HelpStatus { get; init; }

    public CredentialDto[] Credentials { get; init; }
    
    public DateTime CreateDate { get; private set; }

    public PetPhotoDto[] Photos { get; init; }
}