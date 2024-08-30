using CSharpFunctionalExtensions;

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

        public static Result<Credential, string> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return $"Credential {nameof(name)} can't be empty";

            if (string.IsNullOrWhiteSpace(description))
                return $"Credential {nameof(description)} can't be empty";

            return new Credential(name, description);
        }
    }
}
