namespace PetHomeFinder.Accounts.Domain;

public class ParticipantAccount
{
    public const string PARTICIPANT = nameof(PARTICIPANT);

    private ParticipantAccount()
    {
    }

    public ParticipantAccount(User user)
    {
        Id = Guid.NewGuid();
        User = user;
        UserId = user.Id;
    }

    public Guid Id { get; }

    public Guid UserId { get; }

    public User User { get; }
}