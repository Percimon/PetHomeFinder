using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Application.Abstractions;

public interface IParticipantAccountManager
{
    Task CreateParticipantAccount(ParticipantAccount account, CancellationToken cancellationToken = default);
}