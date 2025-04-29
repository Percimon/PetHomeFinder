using AutoFixture;
using PetHomeFinder.Volunteers.Application.Commands.AddPet;
using PetHomeFinder.Volunteers.Application.Commands.Create;
using PetHomeFinder.Volunteers.Application.Commands.UpdateMainInfo;
using PetHomeFinder.Volunteers.Application.Commands.UpdatePet;
using PetHomeFinder.Volunteers.Application.Queries.GetPetsWithPagination;

namespace PetHomeFinder.Volunteers.IntegrationTests;

public static class FixtureExtensions
{
    public static CreateVolunteerCommand CreateCreateVolunteerCommand(
        this Fixture fixture)
    {
        return fixture.Build<CreateVolunteerCommand>().Create();
    }
    
    public static AddPetCommand CreateAddPetCommand(
        this IFixture fixture,
        Guid volunteerId,
        Guid speciesId,
        Guid breedId)
    {
        DateTime dateOfBirth = DateTime.UtcNow;
        
        return fixture.Build<AddPetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.SpeciesId, speciesId)
            .With(c => c.BreedId, breedId)
            .With(c => c.BirthDate, dateOfBirth)
            .With(c => c.CreateDate, dateOfBirth)
            .With(p => p.HelpStatus, "NEED_TREATMENT")
            .Create();
    }
    
    public static UpdateMainInfoCommand CreateUpdateMainInfoCommand(
        this IFixture fixture,
        Guid volunteerId,
        int experience)
    {
        return fixture.Build<UpdateMainInfoCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.Experience, experience)
            .Create();
    }
    
    public static UpdatePetCommand CreateUpdatePetCommand(
        this IFixture fixture,
        Guid volunteerId, 
        Guid petId,
        Guid speciesId,
        Guid breedId,
        string description)
    {
        DateTime dateOfBirth = DateTime.UtcNow;
        
        return fixture.Build<UpdatePetCommand>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.PetId, petId)
            .With(c => c.SpeciesId, speciesId)
            .With(c => c.BreedId, breedId)
            .With(c => c.Description, description)
            .With(c => c.BirthDate, dateOfBirth)
            .Create();
    }
    
    public static GetPetsWithPaginationQuery CreateGetPetsWithPaginationQuery(
        this IFixture fixture,
        Guid volunteerId, 
        Guid speciesId,
        Guid breedId,
        string name)
    {
        DateTime dateOfBirth = DateTime.UtcNow;
        
        return fixture.Build<GetPetsWithPaginationQuery>()
            .With(c => c.VolunteerId, volunteerId)
            .With(c => c.SpeciesId, speciesId)
            .With(c => c.BreedId, breedId)
            .With(c => c.Name, name)
            .Without(c => c.IsCastrated)
            .Without(c => c.IsVaccinated)
            .Without(c => c.OlderThan)
            .Without(c => c.YoungerThan)
            .Create();
    }
    
}