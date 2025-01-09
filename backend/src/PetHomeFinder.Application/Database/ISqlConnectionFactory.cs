using System.Data;

namespace PetHomeFinder.Application.Database;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}