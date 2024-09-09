using System;

namespace PetHomeFinder.Application.DTOs;

public record FullNameDto(
    string FirstName,
    string LastName,
    string Surname);