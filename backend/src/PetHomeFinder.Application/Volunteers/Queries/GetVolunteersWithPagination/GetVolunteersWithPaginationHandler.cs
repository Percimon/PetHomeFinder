namespace PetHomeFinder.Application.Volunteers.Queries.GetVolunteersWithPagination;

public class GetVolunteersWithPaginationHandler
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetVolunteersWithPaginationHandler(IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task Handle(CancellationToken cancellationToken)
    {
         
    }
}