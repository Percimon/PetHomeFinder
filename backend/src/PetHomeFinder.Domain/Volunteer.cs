using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHomeFinder.Domain
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
        public List<SocialNetwork> SocialNetworks { get; private set; }
        public List<Credential> Credentials { get; private set; }
        public List<Pet> PetsOwning { get; private set; }
    }
}
