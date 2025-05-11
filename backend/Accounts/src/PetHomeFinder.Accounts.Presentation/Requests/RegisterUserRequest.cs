namespace PetHomeFinder.Accounts.Presentation.Requests;

public record RegisterUserRequest(
    string UserName,
    string Email,
    string Password);