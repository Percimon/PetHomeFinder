namespace PetHomeFinder.Domain.Volunteers
{
    public record SocialNetworkList
    {
        public IReadOnlyList<SocialNetwork> SocialNetworks { get; }

        //EF core constructor
        private SocialNetworkList() { }

        public SocialNetworkList(IEnumerable<SocialNetwork> socialNetworks)
        {
            SocialNetworks = socialNetworks.ToList();
        }
    }

}
