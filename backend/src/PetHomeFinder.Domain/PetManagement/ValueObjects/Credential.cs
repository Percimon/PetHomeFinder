namespace PetHomeFinder.Domain.Shared
{
    public record Credential
    {
        public string Name { get; }
        public string Description { get; }
        private Credential(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public static Credential Create(string name, string description) =>
            new Credential(name, description);
    }
}
