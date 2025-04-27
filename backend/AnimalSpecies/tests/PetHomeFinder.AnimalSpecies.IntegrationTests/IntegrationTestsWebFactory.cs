using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Infrastructure.DbContexts;
using Respawn;
using Testcontainers.PostgreSql;

namespace PetHomeFinder.AnimalSpecies.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private Respawner _respawner = null;

    private DbConnection _dbConnection = null;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet-home-finder-tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices);
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        services.RemoveAll(typeof(VolunteersWriteDbContext));
        
        services.RemoveAll(typeof(SpeciesWriteDbContext));
        
        services.RemoveAll(typeof(IVolunteersReadDbContext));
        
        services.RemoveAll(typeof(ISpeciesReadDbContext));
        
        services.AddScoped<VolunteersWriteDbContext>(_ =>
            new VolunteersWriteDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<SpeciesWriteDbContext>(_ =>
            new SpeciesWriteDbContext(_dbContainer.GetConnectionString()));

        services.AddScoped<IVolunteersReadDbContext>(_ =>
            new VolunteersVolunteersReadDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<ISpeciesReadDbContext>(_ =>
            new SpeciesSpeciesReadDbContext(_dbContainer.GetConnectionString()));
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions { DbAdapter = DbAdapter.Postgres, SchemasToInclude = ["public"] });
    }
    
    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        
        var volunteersWriteDbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
        await volunteersWriteDbContext.Database.MigrateAsync();
        
        var speciesWriteDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
        await speciesWriteDbContext.Database.MigrateAsync();
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        
        await InitializeRespawnerAsync();
    }
    
    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }
    
    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}