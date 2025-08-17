using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Domain;

public class AdminAccount
{
    public const string ADMIN = nameof(ADMIN);
    
    private AdminAccount(){}
    
    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName; 
        User = user;
    }
    
    public Guid Id { get; }
    
    public Guid UserId { get; }

    public User User { get; }
    
    public FullName FullName { get; }
}