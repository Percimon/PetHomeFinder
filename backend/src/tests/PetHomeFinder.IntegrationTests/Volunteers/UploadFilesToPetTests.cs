using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Application.Volunteers.Commands.UploadFilesToPet;

namespace PetHomeFinder.IntegrationTests.Volunteers;

public class UploadFilesToPetTests : VolunteerTestsBase
{
    private readonly ICommandHandler<Guid, UploadFilesToPetCommand> _sut;

    public UploadFilesToPetTests(VolunteerTestsWebFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider.GetRequiredService<ICommandHandler<Guid, UploadFilesToPetCommand>>();
    }

    [Fact]
    public async Task Upload_files_to_pet_should_be_successful()
    {
        Factory.SetupUploadSuccessMock();

        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var filesDto = new UploadFileDto[] { new UploadFileDto(Stream.Null, "test-photo.jpg") };

        var command = new UploadFilesToPetCommand(volunteerId, pet, filesDto);

        var result = await _sut.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var photo = WriteDbContext.Volunteers
            .ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId)
            .PetsOwning
            .FirstOrDefault(p => p.Id.Value == pet)
            .Photos;

        photo.Count.Should().Be(filesDto.Length);
    }

    [Fact]
    public async Task Upload_files_to_pet_should_be_failed()
    {
        Factory.SetupUploadFailureMock();

        var volunteerId = await SeedVolunteerAsync();

        var species = await SeedSpeciesAsync();

        var breedId = await SeedBreedAsync(species);

        var volunteer = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId);

        var pet = await SeedPetAsync(volunteer, species.Id, breedId);

        var filesDto = new UploadFileDto[] { new UploadFileDto(Stream.Null, "test-photo.jpg") };

        var command = new UploadFilesToPetCommand(volunteerId, pet, filesDto);

        var result = await _sut.Handle(command, CancellationToken.None);
        
        result.IsFailure.Should().BeTrue();
        
        var photo = WriteDbContext.Volunteers.ToList()
            .FirstOrDefault(x => x.Id.Value == volunteerId)
            .PetsOwning
            .FirstOrDefault(p => p.Id.Value == pet)
            .Photos;

        int fileCountRequirement = 0;
        
        photo.Count.Should().Be(fileCountRequirement);
    }
}