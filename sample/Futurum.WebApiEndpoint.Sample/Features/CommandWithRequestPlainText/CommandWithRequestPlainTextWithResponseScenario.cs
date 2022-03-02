using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText<ApiEndpoint>.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestPlainText query, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {query.Body}"))
                      .First()
                      .ToResultOkAsync();
    }
}