namespace PetHomeFinder.Domain.Shared
{
    public record CredentialList
    {
        public IReadOnlyList<Credential> Credentials { get; }

        //EF core constructor
        private CredentialList() { }

        public CredentialList(IEnumerable<Credential> credentials)
        {
            Credentials = credentials.ToList();
        }
    }

}
