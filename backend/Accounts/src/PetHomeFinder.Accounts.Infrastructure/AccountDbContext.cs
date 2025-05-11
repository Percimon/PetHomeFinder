using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Accounts.Domain;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Accounts.Infrastructure;

public class AccountDbContext :
    IdentityDbContext<User, Role, Guid>
{
    private readonly IConfiguration _configuration;
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    
    public AccountDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString(Constants.DATABASE));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .ToTable("users");

        builder.Entity<Role>()
            .ToTable("roles");

        builder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("user_claims");

        builder.Entity<IdentityUserToken<Guid>>()
            .ToTable("user_tokens");

        builder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("user_logins");

        builder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("role_claims");

        builder.Entity<IdentityUserRole<Guid>>()
            .ToTable("user_roles");
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());
}