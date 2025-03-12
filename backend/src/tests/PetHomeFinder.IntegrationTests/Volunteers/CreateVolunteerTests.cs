using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.Volunteers.Commands.Create;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Infrastructure.DbContexts;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class CreateVolunteerTests : IClassFixture<IntegrationTestsWebFactory>, IAsyncLifetime
{
    private readonly Fixture _fixture;
    private readonly IntegrationTestsWebFactory _factory;
    private readonly WriteDbContext _writeDbContext;
    private readonly IReadDbContext _readDbContext;
    private readonly IServiceScope _scope;
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;
    
    public CreateVolunteerTests(IntegrationTestsWebFactory factory)
    {
        _fixture = new Fixture();
        _factory = factory;
        _scope = _factory.Services.CreateScope();
        _writeDbContext = _scope.ServiceProvider.GetRequiredService<WriteDbContext>();
        _readDbContext = _scope.ServiceProvider.GetRequiredService <IReadDbContext>();
        _sut = _scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Create_volunteer()
    { 
        var command = _fixture.CreateCreateVolunteerCommand();

        var result = await _sut.Handle(command, CancellationToken.None);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        
        var volunteer = _writeDbContext.Volunteers.FirstOrDefault();
        volunteer.Should().NotBeNull();
    }

    private async Task<Guid> SeedVolunteer()
    {
        var volunteer = new Volunteer(
            VolunteerId.New(),
            FullName.Create("Ivan", "Ivanov", "Ivanovich").Value,
            Description.Create("test-description").Value,
            Experience.Create(1).Value,
            PhoneNumber.Create("123456789").Value,
            Enumerable.Empty<Credential>(),
            Enumerable.Empty<SocialNetwork>());
        
        await _writeDbContext.Volunteers.AddAsync(volunteer);
        
        await _writeDbContext.SaveChangesAsync();
        
        return volunteer.Id;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync()
    {
        _scope.Dispose();
        
        return Task.CompletedTask;
    }
}