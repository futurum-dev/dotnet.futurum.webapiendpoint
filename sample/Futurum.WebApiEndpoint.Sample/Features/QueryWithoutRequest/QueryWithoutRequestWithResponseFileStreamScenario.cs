using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseFileStreamScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseFileStream
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}