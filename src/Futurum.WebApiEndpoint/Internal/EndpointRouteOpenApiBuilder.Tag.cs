using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

public interface IEndpointRouteOpenApiBuilderTag
{
    void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition);
}

internal class EndpointRouteOpenApiBuilderTag : IEndpointRouteOpenApiBuilderTag
{
    public void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (_, metadataTypeDefinition, _, _) = metadataDefinition;

        var apiEndpointType = metadataTypeDefinition.WebApiEndpointType;

        routeHandlerBuilder.WithTags(apiEndpointType.GetSanitizedLastPartOfNamespace());
    }
}