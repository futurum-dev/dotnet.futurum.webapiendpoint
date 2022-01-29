using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponse<FeatureDto, Feature>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFiles query, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {query}"))
                      .First()
                      .ToResultOkAsync();
    }
}