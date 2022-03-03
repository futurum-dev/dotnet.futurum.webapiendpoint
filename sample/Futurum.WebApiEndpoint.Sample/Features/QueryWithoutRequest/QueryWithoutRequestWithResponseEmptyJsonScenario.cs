using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseEmptyJson
    {
        public override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}