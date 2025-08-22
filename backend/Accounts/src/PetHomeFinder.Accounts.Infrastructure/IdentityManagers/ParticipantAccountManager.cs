using PetHomeFinder.Accounts.Application.Abstractions;
using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Infrastructure.IdentityManagers;

public class ParticipantAccountManager : IParticipantAccountManager
{
    private readonly AccountsDbContext _context;

    public ParticipantAccountManager(AccountsDbContext context)
    {
        _context = context;
    }
    
    public async Task CreateParticipantAccount(ParticipantAccount account, CancellationToken cancellationToken = default)
    {
        await _context.ParticipantAccounts.AddAsync(account, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
    }
}