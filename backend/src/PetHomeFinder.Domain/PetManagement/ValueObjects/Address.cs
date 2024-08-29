using System;

namespace PetHomeFinder.Domain.Pets;

public record Address
{
    public string City { get; }
    public string District { get; }
    public string Street { get; }
    public string Structure { get; }

    public Address(string city, string district, string street, string structure)
    {
        City = city;
        District = district;
        Street = street;
        Structure = structure;
    }

    public static Address Create(string city, string district, string street, string structure) =>
        new Address(city, district, street, structure);
}
