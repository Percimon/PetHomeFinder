using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetHomeFinder.Application.Abstractions;
using PetHomeFinder.Application.Database;
using PetHomeFinder.Application.SpeciesBreeds.Commands.Create;
using PetHomeFinder.Domain.Shared;

namespace PetHomeFinder.Application.SpeciesBreeds.Commands.Delete;

public class DeleteSpeciesHandler : ICommandHandler<Guid, DeleteSpeciesCommand>
{
    private readonly IValidator<DeleteSpeciesCommand> _validator;
    private readonly ISpeciesRepository _speciesRepository;
    private readonly IReadDbContext _readDbContext;
    private readonly ILogger<DeleteSpeciesHandler> _logger;

    public DeleteSpeciesHandler(
        IValidator<DeleteSpeciesCommand> validator,
        ISpeciesRepository speciesRepository,
        IReadDbContext readDbContext,
        ILogger<DeleteSpeciesHandler> logger)
    {
        _validator = validator;
        _speciesRepository = speciesRepository;
        _readDbContext = readDbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(
        DeleteSpeciesCommand command,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}