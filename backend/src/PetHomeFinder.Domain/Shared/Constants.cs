namespace PetHomeFinder.Domain.Shared;

public class Constants
{
    public const string DATABASE = "Database";
    public const string BUCKET_NAME_PHOTOS = "photos";
    
    public const int MIN_EXPERIENCE_PARAMETER = 0;
    public const int MIN_PHYSICAL_PARAMETER = 0;
    public const int MAX_LOW_TEXT_LENGTH = 100;
    public const int MAX_HIGH_TEXT_LENGTH = 2000;
    
    public static readonly string[] PERMITTED_PET_STATUS_FOR_VOLUNTEER = ["SEARCH_FOR_HOME", "NEED_TREATMENT"];
}
