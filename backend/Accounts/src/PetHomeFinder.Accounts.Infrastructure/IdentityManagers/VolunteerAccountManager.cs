using PetHomeFinder.Accounts.Application.Abstractions;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.IdentityManagers;

public class VolunteerAccountManager : IVolunteerAccountManager
{
    private readonly AccountsDbContext _context;

    public VolunteerAccountManager(AccountsDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateParticipantAccount(VolunteerAccount account, CancellationToken cancellationToken = default)
    {
        await _context.VolunteerAccounts.AddAsync(account, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}