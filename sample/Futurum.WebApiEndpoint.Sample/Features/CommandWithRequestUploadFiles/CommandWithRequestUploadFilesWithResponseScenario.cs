using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles<ApiEndpoint>.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {command.Files.Single().FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}