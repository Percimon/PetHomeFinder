using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Volunteers
{
    public record SocialNetworkList
    {
        public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

        private SocialNetworkList()
        {
        }
        private SocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks.ToList();
        }

        public static Result<SocialNetworkList> Create(IEnumerable<SocialNetwork> socialNetworks)
        {
            return new SocialNetworkList(socialNetworks);
        }
    }

}
