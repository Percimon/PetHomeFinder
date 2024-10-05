using CSharpFunctionalExtensions;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Domain.PetManagement.ValueObjects;

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

    public static Result<FullName, Error> Create(string firstName, string lastName, string surname)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired("FirstName");

        if (firstName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("FirstName");

        if (string.IsNullOrWhiteSpace(lastName))
            return Errors.General.ValueIsRequired("LastName");

        if (lastName.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("LastName");

        if (surname.Length > Constants.MAX_LOW_TEXT_LENGTH)
            return Errors.General.ValueIsRequired("Surname");

        return new FullName(firstName, lastName, surname);
    }

}
