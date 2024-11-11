using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Application.SpeciesBreeds;
using PetHomeFinder.Domain.Shared;
using PetHomeFinder.Domain.SpeciesManagement.AggregateRoot;
using PetHomeFinder.Domain.SpeciesManagement.IDs;

namespace PetHomeFinder.Infrastructure.Repositories;

public class SpeciesRepository : ISpeciesRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SpeciesRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
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
        
        await _applicationDbContext.Species.AddAsync(species, cancellationToken);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);
        
        return species.Id.Value;
    }

    public async Task<Guid> Save(
        Species species, 
        CancellationToken cancellationToken = default)
    {
        _applicationDbContext.Species.Attach(species);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return species.Id.Value;
    }

    public async Task<Guid> Delete(
        Species species, 
        CancellationToken cancellationToken = default)
    {
        _applicationDbContext.Species.Remove(species);
        await _applicationDbContext.SaveChangesAsync(cancellationToken);

        return species.Id.Value;
    }

    public async Task<Result<Species, Error>> GetById(
        SpeciesId speciesId, 
        CancellationToken cancellationToken = default)
    {
        var species = await _applicationDbContext.Species
            .FirstOrDefaultAsync(v => v.Id == speciesId, cancellationToken);

        if (species is null)
            return Errors.General.NotFound(speciesId);

        return species;
    }

    public async Task<Result<Species, Error>> GetByName(Name name, CancellationToken cancellationToken = default)
    {
        var species = await _applicationDbContext.Species
            .FirstOrDefaultAsync(v => v.Name.Value == name.Value, cancellationToken);

        if (species is null)
            return Errors.General.NotFound();

        return species;
    }
}