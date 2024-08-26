using PetHomeFinder.Domain.Pets;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Volunteers
{
    public class Volunteer : Entity
    {
        public Volunteer(Guid id) : base(id)
        {
        }

        public FullName FullName { get; private set; }
        public Description Description { get; private set; }
        public Experience Experience { get; private set; }
        public PetStatistics PetStatistics { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public SocialNetworkList SocialNetworks { get; private set; }
        public CredentialList Credentials { get; private set; }
        public List<Pet> PetsOwning { get; private set; }
    }

}
