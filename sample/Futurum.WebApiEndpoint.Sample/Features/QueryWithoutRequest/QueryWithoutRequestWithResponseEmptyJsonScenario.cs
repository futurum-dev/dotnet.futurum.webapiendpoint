using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseEmptyJson
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}