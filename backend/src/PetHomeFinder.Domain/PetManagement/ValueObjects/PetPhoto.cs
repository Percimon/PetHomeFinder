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

        public static Result<PetPhoto> Create(string filePath, bool isMain = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return Errors.General.ValueIsRequired("FilePath");

            if (filePath.Length > Constants.MAX_HIGH_TEXT_LENGTH)
                return Errors.General.ValueIsRequired("FilePath");

            return new PetPhoto(filePath, isMain);
        }
    }
}
