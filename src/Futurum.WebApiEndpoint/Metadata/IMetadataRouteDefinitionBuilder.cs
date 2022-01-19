using Futurum.ApiEndpoint.DebugLogger;

namespace Futurum.WebApiEndpoint.Metadata;

internal interface IMetadataRouteDefinitionBuilder
{
    IEnumerable<MetadataRouteDefinition> Build();

    ApiEndpointDebugNode Debug();
}