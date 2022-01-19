using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithoutRequestWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseEmptyJson<ApiEndpoint>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}