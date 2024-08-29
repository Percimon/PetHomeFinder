using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets
{
    public record PetPhoto
    {
        private PetPhoto(string filePath, bool isMain)
        {
            FilePath = filePath;
            IsMain = isMain;
        }

        public string FilePath { get; }
        public bool IsMain { get; }

        public static PetPhoto Create(string filePath, bool isMain = false)
        {
            return new PetPhoto(filePath, isMain);
        }
    }
}
