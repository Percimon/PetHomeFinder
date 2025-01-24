using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.IDs;
using PetHomeFinder.Infrastructure.DbContexts;

namespace PetHomeFinder.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly WriteDbContext _writeDbContext;

    public SpeciesRepository(WriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
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

        await _writeDbContext.Species.AddAsync(species, cancellationToken);

        return species.Id.Value;
    }

    public Guid Save(Species species)
    {
        _writeDbContext.Species.Attach(species);

        return species.Id.Value;
    }

    public Guid Delete(Species species)
    {
        _writeDbContext.Species.Remove(species);

        return species.Id.Value;
    }

    public async Task<Result<Species, Error>> GetById(
        SpeciesId speciesId,
        CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species is null)
            return Errors.General.NotFound(speciesId);

        return species;
    }

    public async Task<Result<Species, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var species = await _writeDbContext.Species
            .FirstOrDefaultAsync(v => v.Name.Value == name.Value, cancellationToken);

        if (species is null)
            return Errors.General.NotFound();

        return species;
    }
}