namespace PetHomeFinder.Domain.Volunteers
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

        public static SocialNetwork Create(string name, string link) => new SocialNetwork(name, link);

    }
}
