using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal static class WebApiEndpointExecutor
{
    public static Task ExecuteAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var endpoint = httpContext.GetEndpoint();

        var routePath = ((RouteEndpoint?)endpoint)?.RoutePattern.RawText;

        var metadataDefinition = endpoint?.Metadata.GetMetadata<MetadataDefinition>();

        return WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, routePath, cancellationToken);
    }
}