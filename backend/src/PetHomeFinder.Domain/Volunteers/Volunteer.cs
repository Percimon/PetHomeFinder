using PetHomeFinder.Domain.Pets;

namespace PetHomeFinder.Domain.Volunteers
{
    public class Volunteer
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Surname { get; private set; }
        public string Description { get; private set; }
        public int Experience { get; private set; }
        public int PetHomeFoundCount { get; private set; }
        public int PetSearchForHomeCount { get; private set; }
        public int PetTreatmentCount { get; private set; }
        public string PhoneNumber { get; private set; }
        public SocialNetworkList SocialNetworks { get; private set; }
        public CredentialList Credentials { get; private set; }
        public List<Pet> PetsOwning { get; private set; }
    }

}
