using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Volunteers.Domain.Entities;

namespace PetHomeFinder.Volunteers.Infrastructure.DbContexts
{
    public class WriteDbContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        
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