namespace PetHomeFinder.Volunteers.Contracts.Requests;

public record GetPetsWithPaginationRequest(
    Guid? VolunteerId,
    Guid? SpeciesId,
    Guid? BreedId,
    string? Name,
    string? Color,
    string? City,
    string? Street,
    string? HouseNumber,
    double? HeightFrom,
    double? HeightTo,
    double? WeightFrom,
    double? WeightTo,
    bool? IsCastrated,
    DateTime? Younger,
    DateTime? Older,
    bool? IsVaccinated,
    string? HelpStatus,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize);