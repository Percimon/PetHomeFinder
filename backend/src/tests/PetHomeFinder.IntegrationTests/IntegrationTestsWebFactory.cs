using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Infrastructure.DbContexts;
using Testcontainers.PostgreSql;

namespace PetHomeFinder.IntegrationTests;

public class IntegrationTestsWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres")
        .WithDatabase("pet_home_finder_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigureDefaultServices); 
    }

    protected virtual void ConfigureDefaultServices(IServiceCollection services)
    {
        var writeDbContext = services.SingleOrDefault(s => 
            s.ServiceType == typeof(WriteDbContext));
        
        var readDbContext = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IReadDbContext));
        
        if(writeDbContext is not null)
            services.Remove(writeDbContext);
        
        if(readDbContext is not null)
            services.Remove(readDbContext);
        
        services.AddScoped<WriteDbContext>(_ =>
            new WriteDbContext(_dbContainer.GetConnectionString()));
        
        services.AddScoped<IReadDbContext>(_ =>
            new ReadDbContext(_dbContainer.GetConnectionString()));
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}