using System;

namespace PetHomeFinder.Domain.Volunteers;

public record FullName
{
    private FullName(string firstName, string lastName, string surname)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string Surname { get; }

    public static FullName Create(string firstName, string lastName, string surname) =>
        new FullName(firstName, lastName, surname);

}
