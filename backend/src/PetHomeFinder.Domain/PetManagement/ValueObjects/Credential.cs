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

        public static Result<Credential> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsRequired("Credential.Name");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsRequired("Credential.Description");

            return new Credential(name, description);
        }
    }
}
