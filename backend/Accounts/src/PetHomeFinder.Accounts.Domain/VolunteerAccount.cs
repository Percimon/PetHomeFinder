using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Domain;

public class VolunteerAccount
{
    public const string VOLUNTEER = nameof(VOLUNTEER);

    private List<Credential> _credentials = [];

    private VolunteerAccount()
    {
    }

    public VolunteerAccount(User user, IEnumerable<Credential> credentials)
    {
        Id = Guid.NewGuid();
        UserId = user.Id;
        User = user;
        _credentials = credentials.ToList();
    }

    public Guid Id { get; }

    public Guid UserId { get; }

    public User User { get; }

    public void UpdateCredentials(IEnumerable<Credential> credentials)
    {
        _credentials = credentials.ToList();
    }

    public IReadOnlyList<Credential> Credentials => _credentials;
}