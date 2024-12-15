using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;

namespace PetHomeFinder.Infrastructure.DbContexts;

public class ReadDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DbSet<VolunteerDto> Volunteers => Set<VolunteerDto>();

    public DbSet<PetDto> Pets => Set<PetDto>();
    
    public ReadDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(Constants.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ReadDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}