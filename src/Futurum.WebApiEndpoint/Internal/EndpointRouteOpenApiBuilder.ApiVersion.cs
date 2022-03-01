using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

public interface IEndpointRouteOpenApiBuilderApiVersion
{
    void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition);
}

internal class EndpointRouteOpenApiBuilderApiVersion : IEndpointRouteOpenApiBuilderApiVersion
{
    private readonly WebApiEndpointConfiguration _configuration;

    public EndpointRouteOpenApiBuilderApiVersion(WebApiEndpointConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var apiVersion = metadataDefinition.MetadataRouteDefinition.ApiVersion ?? _configuration.DefaultApiVersion;
        routeHandlerBuilder.WithGroupName($"v{apiVersion}");
    }
}