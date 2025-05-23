using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Volunteers.Application.Commands.UpdateSocialNetworks;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

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
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var dtos = new SocialNetworkDto[]
        {
            new SocialNetworkDto("name-1", "link-1"),
        };
        
        var command = new UpdateSocialNetworksCommand(volunteerId, dtos);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteer = VolunteersReadDbContext.Volunteers.FirstOrDefault(x => x.Id ==  result.Value);
        
        volunteer.SocialNetworks.Should().NotBeEmpty();
        
        var socialNetork = volunteer.SocialNetworks[0];
        
        socialNetork.Name.Should().Be(dtos[0].Name);
        
        socialNetork.Link.Should().Be(dtos[0].Link);
    }
}