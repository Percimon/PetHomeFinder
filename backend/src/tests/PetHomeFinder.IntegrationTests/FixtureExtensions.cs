using AutoFixture;
using PetHomeFinder.Application.Volunteers.Commands.Create;

namespace PetHomeFinder.IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerCommand CreateCreateVolunteerCommand(
        this Fixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>().Create();
    }
}