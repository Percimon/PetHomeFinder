namespace PetHomeFinder.Domain.Shared
{
    public record CredentialList
    {
        public IReadOnlyList<Credential> Credentials { get; }
        private CredentialList()
        {
        }
        private CredentialList(IEnumerable<Credential> credentials)
        {
            Credentials = credentials.ToList();
        }

        public static Result<CredentialList> Create(IEnumerable<Credential> credentials)
        {
            return new CredentialList(credentials);
        }
    }

}
