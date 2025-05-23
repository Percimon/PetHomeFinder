using CSharpFunctionalExtensions;

namespace PetHomeFinder.SharedKernel.ValueObjects;

public record Address
{
    public string City { get; }
    public string District { get; }
    public string Street { get; }
    public string Structure { get; }

    private Address(string city, string district, string street, string structure)
    {
        City = city;
        District = district;
        Street = street;
        Structure = structure;
    }

    public static Result<Address, Error> Create(
        string city,
        string district,
        string street,
        string structure)
    {
        if (string.IsNullOrWhiteSpace(city))
            return Errors.General.ValueIsRequired("city");

        if (city.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("city");

        if (string.IsNullOrWhiteSpace(district))
            return Errors.General.ValueIsRequired("district");

        if (district.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("district");

        if (string.IsNullOrWhiteSpace(street))
            return Errors.General.ValueIsRequired("street");

        if (street.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("street");

        if (string.IsNullOrWhiteSpace(structure))
            return Errors.General.ValueIsRequired("structure");

        if (structure.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("structure");

        return new Address(city, district, street, structure);
    }
}
