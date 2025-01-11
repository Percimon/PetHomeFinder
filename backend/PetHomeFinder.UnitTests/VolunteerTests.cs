using FluentAssertions;
using PetHomeFinder.Domain.PetManagement.AggregateRoot;
using PetHomeFinder.Domain.PetManagement.Entities;
using PetHomeFinder.Domain.PetManagement.IDs;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void MovePet_WhenNewPositionIsLower_ShouldMoveOtherPetsForward()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var secondPosition = Position.Create(2).Value;

        var firstPet = volunteer.PetsOwning[0];
        var secondPet = volunteer.PetsOwning[1];
        var thirdPet = volunteer.PetsOwning[2];
        var fourthPet = volunteer.PetsOwning[3];
        var fifthPet = volunteer.PetsOwning[4];

        // act
        var result = volunteer.MovePet(fourthPet, secondPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(2);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_WhenNewPositionIsGreater_ShouldMoveOtherPetsBack()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fourthPosition = Position.Create(4).Value;

        var firstPet = volunteer.PetsOwning[0];
        var secondPet = volunteer.PetsOwning[1];
        var thirdPet = volunteer.PetsOwning[2];
        var fourthPet = volunteer.PetsOwning[3];
        var fifthPet = volunteer.PetsOwning[4];

        // act
        var result = volunteer.MovePet(secondPet, fourthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(1);
        secondPet.Position.Value.Should().Be(4);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(5);
    }

    [Fact]
    public void MovePet_WhenNewPositionIsFirst_ShouldMoveOtherPetsForward()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var firstPosition = Position.Create(1).Value;

        var firstPet = volunteer.PetsOwning[0];
        var secondPet = volunteer.PetsOwning[1];
        var thirdPet = volunteer.PetsOwning[2];
        var fourthPet = volunteer.PetsOwning[3];
        var fifthPet = volunteer.PetsOwning[4];

        // act
        var result = volunteer.MovePet(fifthPet, firstPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(2);
        secondPet.Position.Value.Should().Be(3);
        thirdPet.Position.Value.Should().Be(4);
        fourthPet.Position.Value.Should().Be(5);
        fifthPet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void MovePet_WhenNewPositionIsLast_ShouldMoveOtherPetsBack()
    {
        // arrange
        const int petsCount = 5;

        var volunteer = CreateVolunteerWithPets(petsCount);

        var fifthPosition = Position.Create(5).Value;

        var firstPet = volunteer.PetsOwning[0];
        var secondPet = volunteer.PetsOwning[1];
        var thirdPet = volunteer.PetsOwning[2];
        var fourthPet = volunteer.PetsOwning[3];
        var fifthPet = volunteer.PetsOwning[4];

        // act
        var result = volunteer.MovePet(firstPet, fifthPosition);

        // assert
        result.IsSuccess.Should().BeTrue();
        firstPet.Position.Value.Should().Be(5);
        secondPet.Position.Value.Should().Be(1);
        thirdPet.Position.Value.Should().Be(2);
        fourthPet.Position.Value.Should().Be(3);
        fifthPet.Position.Value.Should().Be(4);
    }

    private Volunteer CreateVolunteerWithPets(int petsCount)
    {
        var volunteerId = VolunteerId.New();
        var fullName = FullName.Create("test", "test", "test").Value;
        var description = Description.Create("test").Value;
        var experience = Experience.Create(1).Value;
        var phoneNumber = PhoneNumber.Create("123456789").Value;
        
        var volunteer = new Volunteer(
            volunteerId,
            fullName,
            description,
            experience,
            phoneNumber,
            [],
            []);

        var name = Name.Create("test").Value;
        var speciesId = SpeciesId.New();
        var breedId = BreedId.New();
        var speciesBreed = SpeciesBreed.Create(speciesId, breedId.Value).Value;
        var color = Color.Create("test").Value;
        var health = HealthInfo.Create("test").Value;
        var address = Address.Create("test", "test", "test", "test").Value;
        var weight = Weight.Create(1).Value;
        var height = Height.Create(1).Value;
        var isVaccinated = true;
        var isCastrated = true;
        var birthDate = DateTime.Now;
        var createDate = DateTime.Now;
        var status = HelpStatusEnum.SEARCH_FOR_HOME;
        var petCredentials = new CredentialList(new List<Credential>());

        for (int i = 0; i < petsCount; i++)
        {
            var pet = new Pet(
                PetId.New(),
                name,
                speciesBreed,
                description,
                color,
                health,
                address,
                weight,
                height,
                phoneNumber,
                isCastrated,
                isVaccinated,
                birthDate,
                status,
                petCredentials,
                createDate);

            volunteer.AddPet(pet);
        }

        return volunteer;
    }
}