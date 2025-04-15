using FluentAssertions;
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
    
    [Fact]
    public async Task Update_pet_main_photo_should_work()
    {
        //arrange
        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var newfilePath = "testFile-3.jpg";
        
        var command = new UpdatePetMainPhotoCommand(volunteerId, pet, newfilePath);
        
        //act
        var result = await _sut.Handle(command, CancellationToken.None);

        //assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().NotBeEmpty();

        var photos = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(v => v.Id.Value == volunteerId)
            .PetsOwning
            .FirstOrDefault(p => p.Id.Value == pet)
            .Photos;

        photos.Count.Should().BeGreaterThan(0);

        var mainPhoto = photos.FirstOrDefault(p => p.IsMain);
            
        mainPhoto.Should().NotBeNull();
        
        mainPhoto.FilePath.Should().Be(newfilePath);
    }
}