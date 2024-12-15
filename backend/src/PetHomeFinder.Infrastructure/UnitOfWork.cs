using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Infrastructure.DbContexts;

namespace PetHomeFinder.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReadDbContext _dbContext;

    public UnitOfWork(ReadDbContext dbContext)
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