namespace PetHomeFinder.Accounts.Infrastructure.Options;

public class JwtOptions
{
    public const string JWT = nameof(JWT);

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string Key { get; init; } = string.Empty;

    public int AccessTokenLifetime { get; init; } = 60;
}