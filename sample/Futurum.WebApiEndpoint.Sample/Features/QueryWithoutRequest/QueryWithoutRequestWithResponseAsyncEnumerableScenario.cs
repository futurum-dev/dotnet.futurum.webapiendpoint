using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseAsyncEnumerable<ApiEndpoint, FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i}"))).ToResultOkAsync();
    }
}