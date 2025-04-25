using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHomeFinder.AnimalSpecies.Domain.Entities;

namespace PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts
{
    public class WriteDbContext : DbContext
    {
        private readonly string _connectionString;
        
        public DbSet<Species> Species => Set<Species>();
        
        public WriteDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ReadDbContext).Assembly,
                type => type.FullName?.Contains("Configurations.Write") ?? false);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());
    }
}