using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.DTOs;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteerById;

public class GetVolunteerByIdHandler : IQueryHandler<VolunteerDto, GetVolunteerByIdQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetVolunteerByIdHandler> _logger;

    public GetVolunteerByIdHandler(
        ISqlConnectionFactory sqlConnectionFactory,
        ILogger<GetVolunteerByIdHandler> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _logger = logger;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(
        GetVolunteerByIdQuery query,
        CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();
        parameters.Add("@Id", query.VolunteerId);

        var sql =
            """
            SELECT 
                id, 
                first_name,
                last_name,
                surname,
                description,
                experience,
                phone_number,
                credentials,
                social_networks
            FROM Volunteers WHERE Id = @Id
            """;

        var volunteers = await connection.QueryAsync<VolunteerDto, string, string, VolunteerDto>(
            sql, (volunteer, credentialsJson, socialNetworksJson) =>
            {
                var credentials = JsonSerializer.Deserialize<CredentialDto[]>(credentialsJson) ?? [];
                var socialNetworks = JsonSerializer.Deserialize<SocialNetworkDto[]>(socialNetworksJson) ?? [];

                volunteer.Credentials = credentials;
                volunteer.SocialNetworks = socialNetworks;

                return volunteer;
            },
            splitOn: "credentials, social_networks",
            param: parameters);

        var result = volunteers.FirstOrDefault();
        if (result is null)
            return Errors.General.NotFound().ToErrorList();

        _logger.LogInformation("Volunteer was received with id: {volunteerId}", result.Id);

        return result;
    }
}