using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Application.Abstractions;

public interface IVolunteerAccountManager
{
    Task CreateParticipantAccount(VolunteerAccount account, CancellationToken cancellationToken);
}