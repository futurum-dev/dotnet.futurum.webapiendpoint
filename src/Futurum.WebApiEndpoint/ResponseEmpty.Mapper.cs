using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint;

internal class ResponseEmptyMapper : IWebApiEndpointResponseMapper<ResponseEmpty, ResponseEmptyDto>
{
    public Task<Result> MapAsync(HttpContext httpContext, MetadataRouteDefinition metadataRouteDefinition, ResponseEmpty domain, CancellationToken cancellation)
    {
        httpContext.Response.StatusCode = metadataRouteDefinition.SuccessStatusCode;

        return Result.OkAsync();
    }
}