using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseAsyncEnumerableScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseAsyncEnumerable<FeatureDto, Feature>.Mapper<FeatureDataMapper>
    {
        public override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i}"))).ToResultOkAsync();
    }
}