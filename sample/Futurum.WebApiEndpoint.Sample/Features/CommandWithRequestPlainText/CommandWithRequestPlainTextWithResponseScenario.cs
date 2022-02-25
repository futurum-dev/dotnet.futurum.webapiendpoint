using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestPlainText<ApiEndpoint>.WithResponse<FeatureDto, Feature>.WithMapper<FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestPlainText query, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {query.Body}"))
                      .First()
                      .ToResultOkAsync();
    }
}