namespace PetHomeFinder.Domain.Pets
{
    public class PetPhoto
    {
        public Guid Id { get; private set; }
        public string FilePath { get; private set; }
        public bool IsMain { get; private set; }
    }
}
