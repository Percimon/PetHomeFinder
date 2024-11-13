using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Extensions;
using PetHomeFinder.Domain.PetManagement.ValueObjects;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler
{
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IValidator<UpdateSocialNetworksCommand> _validator;

    public UpdateSocialNetworksHandler(
        IVolunteersRepository volunteersRepository,
        IValidator<UpdateSocialNetworksCommand> validator,
        ILogger<UpdateSocialNetworksHandler> logger)
    {
        _volunteersRepository = volunteersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToErrorList();

        var volunteerResult = await _volunteersRepository.GetById(command.VolunteerId, cancellationToken);
        if (volunteerResult.IsFailure)
            return volunteerResult.Error.ToErrorList();

        var socialNetworks = new ValueObjectList<SocialNetwork>(command
            .SocialNetworkList
            .SocialNetworks
            .Select(r => SocialNetwork.Create(r.Name, r.Link).Value));

        volunteerResult.Value.UpdateSocialNetworks(socialNetworks);

        await _volunteersRepository.Save(volunteerResult.Value, cancellationToken);

        _logger.LogInformation("Social networks of volunteer updated with id: {VolunteerId}.", command.VolunteerId);

        return volunteerResult.Value.Id.Value;
    }
}
