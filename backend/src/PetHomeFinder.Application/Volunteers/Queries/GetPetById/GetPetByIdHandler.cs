using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetPetByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var petQuery = _readDbContext.Pets;
        
        var pet = await petQuery.FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);
        
        if(pet is null)
            return Errors.General.NotFound(query.PetId).ToErrorList();
        
        return pet;
    }
}