using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Volunteers.Application.Commands.Delete;

namespace PetHomeFinder.Volunteers.IntegrationTests.Volunteers;

public class DeleteVolunteerTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, DeleteVolunteerCommand> _sut;
    
    public DeleteVolunteerTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, DeleteVolunteerCommand>>();
    }
    
    [Fact]
    public async Task Delete_volunteer_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();
        
        var command = new DeleteVolunteerCommand(volunteerId);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();
        
        result.Value.Should().NotBeEmpty();

        var volunteerQuery = ReadDbContext.Volunteers
            .Where(v => v.IsDeleted == false)
            .ToList();

        volunteerQuery.Count.Should().Be(0);
    }
}