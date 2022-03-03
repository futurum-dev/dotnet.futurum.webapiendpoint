using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseStreamScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseStream
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), "hello-world-stream").ToResultOkAsync();
    }
}