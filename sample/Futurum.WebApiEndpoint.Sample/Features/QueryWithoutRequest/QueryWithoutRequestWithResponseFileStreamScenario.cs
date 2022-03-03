using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseFileStreamScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestEmpty command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}