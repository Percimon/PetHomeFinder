using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets
{
    public class Pet : Entity<PetId>
    {
        public Pet(PetId id) : base(id)
        {
        }

        public Pet(
            PetId id,
            Name name,
            SpeciesBreed speciesBreed,
            Description description,
            Color color,
            HealthInfo healthInfo,
            Address address,
            Weight weight,
            Height height,
            PhoneNumber ownerPhoneNumber,
            bool isCastrated,
            bool isVaccinated,
            DateTime birthDate,
            HelpStatusEnum helpStatus,
            CredentialList credentials,
            DateTime createDate) : base(id)
        {
            Name = name;
            SpeciesBreed = speciesBreed;
            Description = description;
            Color = color;
            HealthInfo = healthInfo;
            Address = address;
            Weight = weight;
            Height = height;
            OwnerPhoneNumber = ownerPhoneNumber;
            IsCastrated = isCastrated;
            IsVaccinated = isVaccinated;
            BirthDate = birthDate;
            HelpStatus = helpStatus;
            Credentials = credentials;
            CreateDate = createDate;
        }

        public Name Name { get; private set; }
        public SpeciesBreed SpeciesBreed { get; private set; }
        public Description Description { get; private set; }
        public Color Color { get; private set; }
        public HealthInfo HealthInfo { get; private set; }
        public Address Address { get; private set; }
        public Weight Weight { get; private set; }
        public Height Height { get; private set; }
        public PhoneNumber OwnerPhoneNumber { get; private set; }
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateTime BirthDate { get; private set; }
        public HelpStatusEnum HelpStatus { get; private set; }
        public CredentialList Credentials { get; private set; }
        public DateTime CreateDate { get; private set; }
        public PetPhotoList Photos { get; private set; }
    }

    public enum HelpStatusEnum
    {
        NEED_TREATMENT,
        SEARCH_FOR_HOME,
        FOUND_HOME
    }

}