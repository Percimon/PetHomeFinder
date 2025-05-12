using PetHomeFinder.Accounts.Domain;

namespace PetHomeFinder.Accounts.Application;

public interface ITokenProvider
{
    string GenerateToken(User user);
}