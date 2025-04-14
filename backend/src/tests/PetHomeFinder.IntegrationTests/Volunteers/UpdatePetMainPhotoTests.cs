using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Volunteers.Commands.UpdatePetMainPhoto;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UpdatePetMainPhotoTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UpdatePetMainPhotoCommand> _sut;
    
    public UpdatePetMainPhotoTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UpdatePetMainPhotoCommand>>();
    }
}