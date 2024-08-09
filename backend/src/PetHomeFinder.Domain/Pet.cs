namespace PetHomeFinder.Domain
{
    public class Pet
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string AnimalType { get; private set; } 
        public string Description { get; private set; }
        public string Breed { get; private set; }
        public string Color { get; private set; }
        public string HealthInfo { get; private set; }
        public string Address { get; private set; }
        public double Weight { get; private set; }
        public double Height { get; private set; }
        public string OwnerPhoneNumber { get; private set; }
        public bool IsCastrated { get; private set; }
        public bool IsVaccinated { get; private set; }
        public DateTime BirthDate { get; private set; }
        public HelpStatusEnum HelpStatus { get; private set; }
        public List<Credential> Credentials { get; private set; }
        public DateTime CreateDate { get; private set; }
    }

    public enum HelpStatusEnum
    {
        NEED_HELP,
        SEARCH_FOR_HOME,
        FOUND_HOME
    }

}