using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Domain;

public class AdminAccount
{
    public const string Admin = nameof(Admin);
    
    private AdminAccount(){}
    
    public AdminAccount(FullName fullName, User user)
    {
        Id = Guid.NewGuid();
        FullName = fullName; 
        User = user;
        UserId = user.Id;
    }
    
    public Guid Id { get; }
    
    public Guid UserId { get; }

    public User User { get; }
    
    public FullName FullName { get; }
}