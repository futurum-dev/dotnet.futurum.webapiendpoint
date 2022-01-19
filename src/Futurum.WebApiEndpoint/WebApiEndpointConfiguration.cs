using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;

namespace Futurum.WebApiEndpoint;

/// <summary>
/// WebApiEndpoint configuration
/// </summary>
public record WebApiEndpointConfiguration(Func<WebApiEndpointConfiguration, MetadataRouteDefinition, string> RouteFactory, ApiVersion DefaultApiVersion, bool SecureByDefault, bool EnableMiddleware)
{
    /// <summary>
    /// Default WebApiEndpoint configuration
    /// </summary>
    public static WebApiEndpointConfiguration Default
    {
        get
        {
            string RouteFactory(WebApiEndpointConfiguration configuration, MetadataRouteDefinition metadataRouteDefinition) =>
                metadataRouteDefinition.ApiVersion == null
                    ? $"api/{configuration.DefaultApiVersion}/{metadataRouteDefinition.RouteTemplate}"
                    : $"api/{metadataRouteDefinition.ApiVersion}/{metadataRouteDefinition.RouteTemplate}";

            return new(RouteFactory, new(1, 0), false, false);
        }
    }
}