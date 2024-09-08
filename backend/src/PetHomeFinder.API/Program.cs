using PetHomeFinder.Application.Volunteers;
using PetHomeFinder.Application.Volunteers.CreateVolunteer;
using PetHomeFinder.Infrastructure;
using PetHomeFinder.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<CreateVolunteerHandler>();
builder.Services.AddScoped<IVolunteerRepository, VolunteerRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
