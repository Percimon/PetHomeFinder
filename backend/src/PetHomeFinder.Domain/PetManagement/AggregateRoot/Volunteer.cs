using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Pets;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Volunteers
{
    public class Volunteer : Entity<VolunteerId>
    {
        public Volunteer(VolunteerId id) : base(id)
        {
        }

        public Volunteer(
            VolunteerId id,
            FullName fullName,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber,
            CredentialList credentials,
            SocialNetworkList socialNetworks) : base(id)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
            Credentials = credentials;
            SocialNetworks = socialNetworks;
        }

        public FullName FullName { get; private set; }
        public Description Description { get; private set; }
        public Experience Experience { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public SocialNetworkList SocialNetworks { get; private set; }
        public CredentialList Credentials { get; private set; }
        public List<Pet> PetsOwning { get; private set; }

        public int GetPetHomeFoundCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.FOUND_HOME).Count();
        public int GetPetSearchForHomeCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.SEARCH_FOR_HOME).Count();
        public int GetPetInTreatmentCount() =>
            PetsOwning.Where(p => p.HelpStatus == HelpStatusEnum.NEED_TREATMENT).Count();

        public void UpdateMainInfo(
            FullName fullName,
            Description description,
            Experience experience,
            PhoneNumber phoneNumber)
        {
            FullName = fullName;
            Description = description;
            Experience = experience;
            PhoneNumber = phoneNumber;
        }

        public void UpdateCredentials(IEnumerable<Credential> credentials)
        {
            Credentials = new CredentialList(credentials);
        }

        public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = new SocialNetworkList(socialNetworks);
        }

    }

}
