using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets
{
    public class Pet : Entity
    {
        public Pet(Guid id) : base(id)
        {
        }

        public Name Name { get; private set; }
        public Species SpeciesId { get; private set; }
        public Description Description { get; private set; }
        public Guid BreedId { get; private set; }
        public Color Color { get; private set; }
        public HealthInfo HealthInfo { get; private set; }
        public Address Address { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; }
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateTime BirthDate { get; private set; }
        public HelpStatusEnum HelpStatus { get; private set; }
        public CredentialList Credentials { get; private set; }
        public DateTime CreateDate { get; private set; }
        public List<PetPhoto> Photos { get; private set; }
    }

    public enum HelpStatusEnum
    {
        NEED_TREATMENT,
        SEARCH_FOR_HOME,
        FOUND_HOME
    }

}