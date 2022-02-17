using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithoutRequestWithResponseDataCollectionScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseDataCollection<ApiEndpoint, FeatureDto, Feature>.WithMapper<FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i}"))).ToResultOkAsync();
    }
}