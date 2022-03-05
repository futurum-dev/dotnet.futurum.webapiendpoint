using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

public static class OpenApiRequestParameterForManualParameterConfiguration
{
    public static IEnumerable<OpenApiParameter> Execute(IEnumerable<MetadataRouteParameterDefinition> metadataRouteParameterDefinitions)
    {
        ParameterLocation TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(MetadataRouteParameterDefinitionType parameterDefinitionType) =>
            parameterDefinitionType switch
            {
                MetadataRouteParameterDefinitionType.Path   => ParameterLocation.Path,
                MetadataRouteParameterDefinitionType.Query  => ParameterLocation.Query,
                MetadataRouteParameterDefinitionType.Cookie => ParameterLocation.Cookie,
                MetadataRouteParameterDefinitionType.Header => ParameterLocation.Header,
                _                                           => throw new ArgumentOutOfRangeException(nameof(parameterDefinitionType), parameterDefinitionType, null)
            };

        return metadataRouteParameterDefinitions.Select(parameterDefinition => new OpenApiParameter
        {
            Name = parameterDefinition.Name,
            Schema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(parameterDefinition.Type),
            Required = WebApiEndpointDotnetTypeToOpenApiIsRequiredMapper.Execute(parameterDefinition.Type),
            In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.ParameterDefinitionType),
        });
    }
}