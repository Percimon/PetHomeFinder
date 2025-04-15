using Docker.DotNet;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.UpdateCredentials;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UpdateCredentialsTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateCredentialsCommand> _sut;

    public UpdateCredentialsTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateCredentialsCommand>>();
    }
    
    [Fact]
    public async Task Update_credentials_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var dtos = new CredentialDto[]
        {
            new CredentialDto("n-1", "d-1"),
        };
        
        var command = new UpdateCredentialsCommand(volunteerId, dtos);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteer = ReadDbContext.Volunteers.FirstOrDefault(x => x.Id == result.Value);
        
        volunteer.Credentials.Should().NotBeEmpty();
        
        var credentials = volunteer.Credentials[0];
        
        credentials.Name.Should().Be(dtos[0].Name);
        
        credentials.Description.Should().Be(dtos[0].Description);
    }
}