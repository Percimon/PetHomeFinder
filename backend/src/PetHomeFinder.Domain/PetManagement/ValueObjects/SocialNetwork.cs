using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects
{
    public record SocialNetwork
    {
        private SocialNetwork(string name, string link)
        {
            Name = name;
            Link = link;
        }

        public string Name { get; }
        public string Link { get; }

        public static Result<SocialNetwork, Error> Create(string name, string link)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Errors.General.ValueIsRequired("Name");

            if (name.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("Name");

            if (string.IsNullOrWhiteSpace(link))
                return Errors.General.ValueIsRequired("Link");

            if (link.Length > Constants.MAX_LOW_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("Link");

            return new SocialNetwork(name, link);
        }
    }
}
