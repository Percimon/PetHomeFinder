using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Application.Abstractions;

public interface ITokenProvider
{
    Task<string> GenerateToken(User user);
}