using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseBytesScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseBytes
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"))
                {
                    FileName = "hello-world-bytes"
                }
                .ToResultOkAsync();
    }
}