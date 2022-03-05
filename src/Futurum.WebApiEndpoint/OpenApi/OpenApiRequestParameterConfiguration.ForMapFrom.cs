using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

public static class OpenApiRequestParameterForMapFromConfiguration
{
    public static IEnumerable<OpenApiParameter> Execute(MetadataMapFromDefinition metadataRouteParameterDefinitions)
    {
        ParameterLocation TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(MapFrom mapFrom) =>
            mapFrom switch
            {
                MapFrom.Path   => ParameterLocation.Path,
                MapFrom.Query  => ParameterLocation.Query,
                MapFrom.Cookie => ParameterLocation.Cookie,
                MapFrom.Header => ParameterLocation.Header,
                _              => throw new ArgumentOutOfRangeException(nameof(mapFrom), mapFrom, null)
            };

        return metadataRouteParameterDefinitions.MapFromParameterDefinitions
                                                .Select(parameterDefinition => new OpenApiParameter
                                                {
                                                    Name = parameterDefinition.Name,
                                                    Schema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(parameterDefinition.PropertyInfo.PropertyType),
                                                    Required = WebApiEndpointDotnetTypeToOpenApiIsRequiredMapper.Execute(parameterDefinition.PropertyInfo.PropertyType),
                                                    In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.MapFromAttribute.MapFrom),
                                                });
    }
}