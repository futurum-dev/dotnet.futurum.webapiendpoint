using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IWebApiEndpointHttpContextDispatcher
{
    Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default);
}

internal class WebApiEndpointHttpContextDispatcher : IWebApiEndpointHttpContextDispatcher
{
    private readonly IOptions<JsonOptions> _serializationOptions;

    public WebApiEndpointHttpContextDispatcher(IOptions<JsonOptions> serializationOptions)
    {
        _serializationOptions = serializationOptions;
    }
    
    public Task<Result> HandleFailedResponseAsync(HttpContext httpContext, IResultError error, MetadataRouteDefinition metadataRouteDefinition, CancellationToken cancellation = default)
    {
        var errorResponse = error.ToErrorStructure();

        return httpContext.Response.TryWriteAsJsonAsync(errorResponse, _serializationOptions.Value.SerializerOptions, metadataRouteDefinition.FailedStatusCode, cancellation);
    }
}