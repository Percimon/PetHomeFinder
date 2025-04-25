namespace PetHomeFinder.Volunteers.Infrastructure.Options;

public class MinioOptions
{
    public const string SECTION_NAME = "Minio";
    
    public string Endpoint { get; init; } = string.Empty;
    public string AccessKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public bool WithSSL { get; init; } = false;
}