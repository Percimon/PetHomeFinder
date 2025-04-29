using System.Data;

namespace PetHomeFinder.Core.Abstractions;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}