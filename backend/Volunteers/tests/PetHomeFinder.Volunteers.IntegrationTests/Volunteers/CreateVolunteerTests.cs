using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.Create;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

public class CreateVolunteerTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, CreateVolunteerCommand> _sut;
    
    public CreateVolunteerTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, CreateVolunteerCommand>>();
    }

    [Fact]
    public async Task Create_volunteer_should_work()
    { 
        //arrange
        var command = Fixture.CreateCreateVolunteerCommand();

        //act
        var result = await _sut.Handle(command, CancellationToken.None);
        
        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();
        
        var volunteer = WriteDbContext.Volunteers.FirstOrDefault();
        
        volunteer.Should().NotBeNull();
    }
}