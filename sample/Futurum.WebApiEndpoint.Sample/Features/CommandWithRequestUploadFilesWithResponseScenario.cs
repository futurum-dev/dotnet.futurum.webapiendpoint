using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponse<FeatureDto, Feature>.WithMapper<FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {command.Files.Single().FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}