using System;
using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.Pets;

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

    public static Result<Address, string> Create(string city, string district, string street, string structure)
    {
        if (string.IsNullOrWhiteSpace(city))
            return $"{nameof(city)} can't be empty";

        if (city.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"{nameof(city)} value is too long";

        if (string.IsNullOrWhiteSpace(district))
            return $"{nameof(district)} can't be empty";

        if (district.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"{nameof(district)} value is too long";

        if (string.IsNullOrWhiteSpace(street))
            return $"{nameof(street)} can't be empty";

        if (street.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"{nameof(street)} value is too long";

        if (string.IsNullOrWhiteSpace(structure))
            return $"{nameof(structure)} can't be empty";

        if (structure.Length > Constants.MAX_HIGH_TEXT_LENGTH)
            return $"{nameof(structure)} value is too long";

        return new Address(city, district, street, structure);
    }
}
