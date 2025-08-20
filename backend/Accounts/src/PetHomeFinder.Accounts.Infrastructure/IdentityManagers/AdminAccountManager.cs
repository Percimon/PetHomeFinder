using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.IdentityManagers;

public class AdminAccountManager
{
    private readonly AccountsDbContext _context;

    public AdminAccountManager(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task CreateAdminAccount(AdminAccount adminAccount, CancellationToken cancellationToken = default)
    {
        await _context.AdminAccounts.AddAsync(adminAccount, cancellationToken);
        
        int n = await _context.SaveChangesAsync(cancellationToken);
    }
}