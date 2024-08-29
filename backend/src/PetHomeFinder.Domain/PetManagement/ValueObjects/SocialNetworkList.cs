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

        public static SocialNetworkList Create(IEnumerable<SocialNetwork> socialNetworks) =>
            new SocialNetworkList(socialNetworks);
    }

}
