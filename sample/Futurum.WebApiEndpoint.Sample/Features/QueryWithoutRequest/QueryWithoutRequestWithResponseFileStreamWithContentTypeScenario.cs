using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseFileStreamWithContentTypeScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponseFileStream<ApiEndpoint>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }
}