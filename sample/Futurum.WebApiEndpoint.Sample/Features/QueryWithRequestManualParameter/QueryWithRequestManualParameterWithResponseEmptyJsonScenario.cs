using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseEmptyJsonScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<Request>.ResponseEmptyJson.Mapper<Mapper>
    {
        public override Task<Result<ResponseEmptyJson>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id))
                       .ToResultAsync();
    }
}