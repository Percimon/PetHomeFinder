using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Accounts.Application;
using PetHomeFinder.Accounts.Infrastructure;
using PetHomeFinder.Accounts.Presentation;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Infrastructure;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.AnimalSpecies.Presentation;
using PetHomeFinder.Web;
using PetHomeFinder.Web.Middlewares;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Infrastructure;
using PetHomeFinder.Volunteers.Infrastructure.DbContexts;
using PetHomeFinder.Volunteers.Presentation;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                         ?? throw new ArgumentNullException("Seq"))
            .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
            .CreateLogger();

builder.Services.AddApi();

builder.Services
    .AddAccountsApplication()
    .AddAccountsInfrastructure()
    .AddAccountsPresentation();

builder.Services
    .AddAnimalSpeciesApplication()
    .AddAnimalSpeciesInfrastructure(builder.Configuration)
    .AddAnimalSpeciesPresentation();

builder.Services
    .AddVolunteersApplication()
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersPresentation();

var app = builder.Build();

app.UseExceptionMiddleware();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();

var volunteersDbContext = scope.ServiceProvider.GetRequiredService<VolunteersWriteDbContext>();
await volunteersDbContext.Database.MigrateAsync();

var speciesDbContext = scope.ServiceProvider.GetRequiredService<SpeciesWriteDbContext>();
await speciesDbContext.Database.MigrateAsync();

app.Run();

public partial class Program { }