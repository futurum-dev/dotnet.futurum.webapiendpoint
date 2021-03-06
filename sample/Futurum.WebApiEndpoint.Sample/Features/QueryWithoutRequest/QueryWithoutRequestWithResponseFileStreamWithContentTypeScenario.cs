using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithoutRequest;

public static class QueryWithoutRequestWithResponseFileStreamWithContentTypeScenario
{
    public class ApiEndpoint : QueryWebApiEndpoint.NoRequest.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestEmpty request, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"))
                {
                    ContentType = "image/png"
                }
                .ToResultOkAsync();
    }
}