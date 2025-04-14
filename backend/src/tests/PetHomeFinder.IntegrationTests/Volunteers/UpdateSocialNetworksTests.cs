using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.UpdateSocialNetworks;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UpdateSocialNetworksTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateSocialNetworksCommand> _sut;
    
    public UpdateSocialNetworksTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateSocialNetworksCommand>>();
    }
    
    [Fact]
    public async Task Update_social_networks_info_should_work()
    {
        var volunteerId = await SeedVolunteerAsync();

        var dtos = new SocialNetworkDto[]
        {
            new SocialNetworkDto("name-1", "link-1"),
        };
        
        var command = new UpdateSocialNetworksCommand(volunteerId, dtos);
        
        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteer = ReadDbContext.Volunteers.FirstOrDefault(x => x.Id ==  result.Value);
        
        volunteer.SocialNetworks.Should().NotBeEmpty();
        
        var socialNetork = volunteer.SocialNetworks[0];
        
        socialNetork.Name.Should().Be(dtos[0].Name);
        
        socialNetork.Link.Should().Be(dtos[0].Link);
    }
}