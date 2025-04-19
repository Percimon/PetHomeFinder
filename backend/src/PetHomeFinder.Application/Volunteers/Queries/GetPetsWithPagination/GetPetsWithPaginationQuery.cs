using PetHomeFinder.Application.Abstractions;

namespace PetHomeFinder.Application.Volunteers.Queries.GetPetsWithPagination;

public record GetPetsWithPaginationQuery(
    Guid? VolunteerId,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Name,
    string? Color,
    string? City,
    string? Street,
    string? Structure,
    double? HeightFrom,
    double? HeightTo,
    double? WeightFrom,
    double? WeightTo,
    bool? IsCastrated,
    DateTime? YoungerThan,
    DateTime? OlderThan,
    bool? IsVaccinated,
    string? HelpStatus,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize) : IQuery;