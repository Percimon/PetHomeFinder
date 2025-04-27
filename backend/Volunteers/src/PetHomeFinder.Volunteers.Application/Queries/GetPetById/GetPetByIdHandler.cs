using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Queries.GetPetById;

public class GetPetByIdHandler : IQueryHandler<PetDto, GetPetByIdQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;

    public GetPetByIdHandler(IVolunteersReadDbContext volunteersReadDbContext)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var petQuery = _volunteersReadDbContext.Pets;
        
        var pet = await petQuery.FirstOrDefaultAsync(p => p.Id == query.PetId, cancellationToken);
        
        if(pet is null)
            return Errors.General.NotFound(query.PetId).ToErrorList();
        
        return pet;
    }
}