using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseBytesScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseBytes<ApiEndpoint>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), "hello-world-bytes").ToResultOkAsync();
    }
}