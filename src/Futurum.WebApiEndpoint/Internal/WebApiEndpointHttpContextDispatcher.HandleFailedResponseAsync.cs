using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal partial interface IWebApiEndpointHttpContextDispatcher
{
    Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
}

internal partial class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    public Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        var errorResponse = error.ToErrorStructure();

        return httpContext.Response.WriteAsJsonAsync(errorResponse, _serializationOptions.Value.JsonSerializerOptions, metadataRouteDefinition.FailedStatusCode, cancellation);
    }
}