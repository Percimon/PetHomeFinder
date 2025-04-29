using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.Core.Abstractions;

namespace PetHomeFinder.AnimalSpecies.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly SpeciesWriteDbContext _dbContext;

    public UnitOfWork(SpeciesWriteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        return transaction.GetDbTransaction();
    }

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}