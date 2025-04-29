using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using PetHomeFinder.Core.Files;
using PetHomeFinder.Core.Providers;
using PetHomeFinder.Core.Shared;
using PetHomeFinder.SharedKernel;

namespace PetHomeFinder.Volunteers.IntegrationTests;

public class VolunteerTestsWebFactory : IntegrationTestsWebFactory
{
    private readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    protected override void ConfigureDefaultServices(IServiceCollection services)
    {
        base.ConfigureDefaultServices(services);

        var fileServiceDescriptor = services.SingleOrDefault(s =>
            s.ServiceType == typeof(IFileProvider));

        if (fileServiceDescriptor is not null)
            services.Remove(fileServiceDescriptor);

        services.AddScoped<IFileProvider>(_ => _fileProviderMock);
    }

    public void SetupUploadSuccessMock()
    {
        IReadOnlyList<FilePath> response = new List<FilePath>()
        {
            FilePath.Create("testUrl").Value
        };
        
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success<IReadOnlyList<FilePath>, ErrorList>(response));
    }

    public void SetupUploadFailureMock()
    {
        _fileProviderMock
            .UploadFiles(Arg.Any<IEnumerable<FileData>>(), Arg.Any<CancellationToken>())
            .Returns(Result.Failure<IReadOnlyList<FilePath>, ErrorList>(Errors.General.NotFound()));
    }
}