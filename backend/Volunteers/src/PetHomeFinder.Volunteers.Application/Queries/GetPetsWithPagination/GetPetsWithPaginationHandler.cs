using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using PetHomeFinder.Core.Abstractions;
using PetHomeFinder.Core.Dtos;
using PetHomeFinder.Core.Extensions;
using PetHomeFinder.Core.Models;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.Application.Queries.GetPetsWithPagination;

public class GetPetsWithPaginationHandler : IQueryHandler<PagedList<PetDto>, GetPetsWithPaginationQuery>
{
    private readonly IVolunteersReadDbContext _volunteersReadDbContext;

    public GetPetsWithPaginationHandler(IVolunteersReadDbContext volunteersReadDbContext)
    {
        _volunteersReadDbContext = volunteersReadDbContext;
    }

    public async Task<Result<PagedList<PetDto>, ErrorList>> Handle(
        GetPetsWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var petQuery = _volunteersReadDbContext.Pets;

        Expression<Func<PetDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "name" => (pet) => pet.Name,
            "birthdate" => (pet) => pet.BirthDate,
            "speciesid" => (pet) => pet.SpeciesId,
            "breedid" => (pet) => pet.BreedId,
            "color" => (pet) => pet.Color,
            "city" => (pet) => pet.Address.City,
            "volunteerid" => (pet) => pet.VolunteerId,
            _ => (pet) => pet.Id
        };

        petQuery = query.SortDirection?.ToLower() == "desc"
            ? petQuery.OrderByDescending(keySelector)
            : petQuery.OrderBy(keySelector);

        petQuery = petQuery.WhereIf(
            query.VolunteerId is not null && query.VolunteerId != Guid.Empty,
            p => p.VolunteerId == query.VolunteerId);

        petQuery = petQuery.WhereIf(
            query.SpeciesId is not null && query.SpeciesId != Guid.Empty,
            p => p.SpeciesId == query.SpeciesId);

        petQuery = petQuery.WhereIf(
            query.BreedId is not null && query.BreedId != Guid.Empty,
            p => p.BreedId == query.BreedId);
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Name),
            p => p.Name.Contains(query.Name));
        
        petQuery = petQuery.WhereIf(
            query.YoungerThan != null,
            p => p.BirthDate > query.YoungerThan);

        petQuery = petQuery.WhereIf(
            query.OlderThan != null,
            p => p.BirthDate < query.OlderThan);
        
        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Color),
            p => p.Color.Contains(query.Color));

        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.City),
            p => p.Address.City.Contains(query.City));

        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Street),
            p => p.Address.Street.Contains(query.Street));

        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Structure),
            p => p.Address.Structure.Contains(query.Structure));

        petQuery = petQuery.WhereIf(
            query.HeightFrom != null,
            p => p.Height >= query.HeightFrom);

        petQuery = petQuery.WhereIf(
            query.HeightTo != null,
            p => p.Height < query.HeightTo);

        petQuery = petQuery.WhereIf(
            query.WeightFrom != null,
            p => p.Weight >= query.WeightFrom);

        petQuery = petQuery.WhereIf(
            query.WeightTo != null,
            p => p.Weight < query.WeightTo);

        petQuery = petQuery.WhereIf(
            query.IsCastrated != null,
            p => p.IsCastrated == query.IsCastrated);
        
        petQuery = petQuery.WhereIf(
            query.IsVaccinated != null,
            p => p.IsVaccinated == query.IsVaccinated);

        petQuery = petQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.HelpStatus),
            p => p.HelpStatus.Contains(query.HelpStatus));

        return await petQuery.ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}