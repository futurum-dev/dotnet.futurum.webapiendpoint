using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseStreamScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseStream
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead())
                {
                    FileName = "hello-world-stream"
                }
                .ToResultOkAsync();
    }
}