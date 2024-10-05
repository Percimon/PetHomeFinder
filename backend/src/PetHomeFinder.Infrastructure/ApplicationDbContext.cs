using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.Volunteers;

namespace PetHomeFinder.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private const string DATABASE = "Database";
        private readonly IConfiguration _configuration;

        public DbSet<Volunteer> Volunteers => Set<Volunteer>();
        public DbSet<Species> Species => Set<Species>();

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(DATABASE));
            optionsBuilder.UseSnakeCaseNamingConvention();
            optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private ILoggerFactory CreateLoggerFactory() =>
            LoggerFactory.Create(builder => builder.AddConsole());


    }
}