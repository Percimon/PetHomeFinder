using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.UpdateMainInfo;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

public class UpdateMainInfoTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdateMainInfoCommand> _sut;

    public UpdateMainInfoTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdateMainInfoCommand>>();
    }
    
    [Fact]
    public async Task Update_main_info_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var newExperience = 5;
        
        var command = Fixture.CreateUpdateMainInfoCommand(volunteerId, newExperience);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteer = VolunteersReadDbContext.Volunteers.FirstOrDefault(x => x.Id ==  result.Value);
        
        volunteer.Experience.Should().Be(newExperience);
    }
}