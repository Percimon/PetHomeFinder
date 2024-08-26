namespace PetHomeFinder.Domain.Shared
{
    public record CredentialList
    {
        public IReadOnlyList<Credential> Credentials { get; }

        public CredentialList(IEnumerable<Credential> credentials)
        {
            Credentials = credentials.ToList();
        }
    }

}
