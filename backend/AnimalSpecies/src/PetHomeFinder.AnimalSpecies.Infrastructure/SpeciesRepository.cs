using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.AnimalSpecies.Application;
using PetHomeFinder.AnimalSpecies.Domain.Entities;
using PetHomeFinder.AnimalSpecies.Infrastructure.DbContexts;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects.Ids;

namespace PetHomeFinder.AnimalSpecies.Infrastructure;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly SpeciesWriteDbContext _speciesWriteDbContext;

    public SpeciesRepository(SpeciesWriteDbContext speciesWriteDbContext)
    {
        _speciesWriteDbContext = speciesWriteDbContext;
    }

    public async Task<Result<Guid, Error>> Add(
        Species species,
        CancellationToken cancellationToken = default)
    {
        var result = await GetByName(species.Name, cancellationToken);
        if (result.IsSuccess)
        {
            return Errors.General.AlreadyExists(
                nameof(Species),
                nameof(Species.Name).ToLower(),
                species.Name.Value);
        }

        await _speciesWriteDbContext.Species.AddAsync(species, cancellationToken);

        return species.Id.Value;
    }

    public Guid Save(Species species)
    {
        _speciesWriteDbContext.Species.Attach(species);

        return species.Id.Value;
    }

    public Guid Delete(Species species)
    {
        _speciesWriteDbContext.Species.Remove(species);

        return species.Id.Value;
    }

    public async Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await _speciesWriteDbContext.Species
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species is null)
            return Errors.General.NotFound(speciesId);

        return species;
    }

    public async Task<Result<Species, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var species = await _speciesWriteDbContext.Species
            .FirstOrDefaultAsync(v => v.Name.Value == name.Value, cancellationToken);

        if (species is null)
            return Errors.General.NotFound();

        return species;
    }
}