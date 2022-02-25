using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseFileStreamWithContentTypeScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponseFileStream<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Result<Request> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id));
    }
}