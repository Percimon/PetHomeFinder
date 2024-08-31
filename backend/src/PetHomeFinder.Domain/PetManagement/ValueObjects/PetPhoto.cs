using CSharpFunctionalExtensions;
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

        public static Result<PetPhoto, string> Create(string filePath, bool isMain = false)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return $"File path can't be empty";

            if (filePath.Length > Constants.MAX_HIGH_TEXT_LENGTH)
                return $"File path is too long";

            return new PetPhoto(filePath, isMain);
        }
    }
}
