using PetHomeFinder.Application;
using PetHomeFinder.Application.Volunteers;
using PetHomeFinder.Application.Volunteers.CreateVolunteer;
using PetHomeFinder.Infrastructure;
using PetHomeFinder.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
