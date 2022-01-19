using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithoutRequestWithResponseFileStreamScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseFileStream<ApiEndpoint>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}