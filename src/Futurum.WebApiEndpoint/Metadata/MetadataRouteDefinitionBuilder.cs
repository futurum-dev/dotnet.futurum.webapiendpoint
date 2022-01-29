using Futurum.ApiEndpoint.DebugLogger;

namespace Futurum.WebApiEndpoint.Metadata;

public partial class MetadataRouteDefinitionBuilder : IMetadataRouteDefinitionBuilder
{
    IEnumerable<MetadataRouteDefinition> IMetadataRouteDefinitionBuilder.Build()
    {
        if (_apiVersions.Any())
        {
            var metadataRouteDefinitions =
                _apiVersions.Select(apiVersion => _metadataRouteDefinition with { ApiVersion = apiVersion });

            foreach (var metadataRouteDefinition in metadataRouteDefinitions)
            {
                yield return metadataRouteDefinition;
            }
        }
        else
        {
            yield return _metadataRouteDefinition;
        }
    }

    ApiEndpointDebugNode IMetadataRouteDefinitionBuilder.Debug()
    {
        var apiEndpointDebugNode = new ApiEndpointDebugNode
        {
            Name = $"{_metadataRouteDefinition.RouteTemplate}-{_metadataRouteDefinition.HttpMethod} ({_apiEndpointType.FullName})"
        };

        if (_apiVersions.Any())
        {
            apiEndpointDebugNode.Children = _apiVersions.Select(apiVersion => new ApiEndpointDebugNode { Name = apiVersion.ToString() }).ToList();
        }

        return apiEndpointDebugNode;
    }
}