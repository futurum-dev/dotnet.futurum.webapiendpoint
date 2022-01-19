using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal static class WebApiEndpointExecutor
{
    public static Task ExecuteAsync(HttpContext httpContext, CancellationToken cancellationToken)
    {
        var endpoint = httpContext.GetEndpoint();

        var routePath = ((RouteEndpoint?)endpoint)?.RoutePattern.RawText;

        var metadataDefinition = endpoint?.Metadata.GetMetadata<MetadataDefinition>();
        var configuration = endpoint?.Metadata.GetMetadata<WebApiEndpointConfiguration>();

        return WebApiEndpointExecutorService.ExecuteAsync(httpContext, metadataDefinition, configuration, routePath, cancellationToken);
    }
}