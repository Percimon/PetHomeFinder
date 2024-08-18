using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets
{
    public class PetPhoto : Entity
    {
        public PetPhoto(Guid id) : base(id)
        {
        }

        public string FilePath { get; private set; }
        public bool IsMain { get; private set; }
    }
}
