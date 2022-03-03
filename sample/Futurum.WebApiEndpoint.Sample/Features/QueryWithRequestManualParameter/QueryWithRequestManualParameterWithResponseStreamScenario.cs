using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseStreamScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<Request>.ResponseStream.Mapper<Mapper>
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{query.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id))
                       .ToResultAsync();
    }
}