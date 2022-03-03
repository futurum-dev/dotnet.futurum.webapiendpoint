using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {request.Body}"))
                      .First()
                      .ToResultOkAsync();
    }
}