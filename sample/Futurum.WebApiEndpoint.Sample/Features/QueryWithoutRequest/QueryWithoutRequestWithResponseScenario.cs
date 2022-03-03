using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.Response<FeatureDto, Feature>.Mapper<FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(RequestEmpty command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i}"))
                      .First()
                      .ToResultOkAsync();
    }
}