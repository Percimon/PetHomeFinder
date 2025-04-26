using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Infrastructure;
using PetHomeFinder.AnimalSpecies.Presentation;
using PetHomeFinder.Web;
using PetHomeFinder.Web.Middlewares;
using PetHomeFinder.Volunteers.Application;
using PetHomeFinder.Volunteers.Infrastructure;
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

builder.Services.AddVolunteersApplication()
    .AddVolunteersInfrastructure(builder.Configuration)
    .AddVolunteersPresentation();

builder.Services.AddAnimalSpeciesApplication()
    .AddAnimalSpeciesInfrastructure(builder.Configuration)
    .AddAnimalSpeciesPresentation();

builder.Services.AddApi();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

// await using var scope = app.Services.CreateAsyncScope();
// var dbContext = scope.ServiceProvider.GetRequiredService<WriteDbContext>();
// await dbContext.Database.MigrateAsync();

app.Run();