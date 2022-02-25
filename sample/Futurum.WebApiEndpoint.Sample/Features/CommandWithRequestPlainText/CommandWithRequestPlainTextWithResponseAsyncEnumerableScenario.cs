using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestPlainText<ApiEndpoint>.WithResponseAsyncEnumerable<FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken)
        {
            return new ResponseAsyncEnumerable<Feature>(AsyncEnumerable()).ToResultOkAsync();

            async IAsyncEnumerable<Feature> AsyncEnumerable()
            {
                await Task.Yield();

                foreach (var i in Enumerable.Range(0, 10))
                {
                    yield return new Feature($"Name - {i} - {command.Body}");
                }
            }
        }
    }
}