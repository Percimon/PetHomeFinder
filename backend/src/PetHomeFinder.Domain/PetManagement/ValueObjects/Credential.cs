using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects
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

        public static Result<Credential, Error> Create(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsRequired("Credential.Name");

            if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("Credential.Name");

            if (string.IsNullOrWhiteSpace(description))
                return Errors.General.ValueIsRequired("Credential.Description");

            if (description.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("Credential.Description");

            return new Credential(name, description);
        }
    }
}
