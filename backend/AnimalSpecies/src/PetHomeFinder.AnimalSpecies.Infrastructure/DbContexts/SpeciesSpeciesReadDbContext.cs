using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.Core.Dtos;

namespace PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;

public class SpeciesSpeciesReadDbContext : DbContext, ISpeciesReadDbContext
{
    private readonly string _connectionString;

    public IQueryable<SpeciesDto> Species => Set<SpeciesDto>();

    public IQueryable<BreedDto> Breeds => Set<BreedDto>();

    public SpeciesSpeciesReadDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());

        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SpeciesSpeciesReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}