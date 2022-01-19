using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestPlainTextWithResponseDataCollectionScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestPlainText<ApiEndpoint>.WithResponseDataCollection<FeatureDto, Feature>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Body}"))).ToResultOkAsync();
    }
}