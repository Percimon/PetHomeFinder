namespace PetHomeFinder.Accounts.Presentation.Requests;

public record RegisterRequest(
    string UserName,
    string Email,
    string Password);