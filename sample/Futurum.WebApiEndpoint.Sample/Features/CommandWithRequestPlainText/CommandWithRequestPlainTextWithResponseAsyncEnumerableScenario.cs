using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestPlainText<ApiEndpoint>.WithResponseAsyncEnumerable<FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Body}"))).ToResultOkAsync();
    }
}