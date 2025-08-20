using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using PetHomeFinder.SharedKernel;
using PetHomeFinder.SharedKernel.ValueObjects;

namespace PetHomeFinder.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    private List<Role> _roles = [];

    private List<SocialNetwork> _socialNetworks = [];

    private User()
    {
        
    }
    
    private User(
        string userName,
        string email,
        Role role,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        UserName = userName;
        Email = email;
        _roles = [role];
        _socialNetworks = socialNetworks.ToList();
    }

    public IReadOnlyList<Role> Roles => _roles;
    
    public AdminAccount? AdminAccount { get; private set; }

    public VolunteerAccount? VolunteerAccount { get; private set; }

    public ParticipantAccount? ParticipantAccount { get; private set; }
    
    public IReadOnlyList<SocialNetwork> SocialNetworks => _socialNetworks;

    public void UpdateSocialNetworks(IEnumerable<SocialNetwork> socialNetworks)
    {
        _socialNetworks = socialNetworks.ToList();
    }

    public static Result<User, Error> CreateAdmin(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != AdminAccount.Admin)
            return Errors.General.ValueIsInvalid("Role");
        
        return new User(
            userName,
            email,
            role,
            []);
    }
    
    public static Result<User, Error> CreateParticipant(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != ParticipantAccount.PARTICIPANT)
            return Errors.General.ValueIsInvalid("Role");
        
        return new User(
            userName,
            email,
            role,
            []);
    }
    
    public static Result<User, Error>  CreateVolunteer(
        string userName,
        string email,
        Role role)
    {
        if (role.Name != VolunteerAccount.VOLUNTEER)
            return Errors.General.ValueIsInvalid("Role");
        
        return new User(
            userName,
            email,
            role,
            []);
    }
}