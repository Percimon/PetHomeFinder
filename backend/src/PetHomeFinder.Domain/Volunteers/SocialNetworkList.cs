namespace PetHomeFinder.Domain.Volunteers
{
    public record SocialNetworkList
    {
        public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

        public SocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks.ToList();
        }
    }

}
