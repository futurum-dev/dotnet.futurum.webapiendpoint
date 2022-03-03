using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

public static class OpenApiRequestParameterConfiguration
{
    public static void ConfigureParameters(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        if (metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions.Any())
        {
            var openApiParameters = GetOpenApiParametersForManualParameterDefinitions(metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }

        if (metadataDefinition.MetadataMapFromDefinition != null)
        {
            var openApiParameters = GetOpenApiParametersForMapFrom(metadataDefinition.MetadataMapFromDefinition);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }
    }

    private static IEnumerable<OpenApiParameter> GetOpenApiParametersForManualParameterDefinitions(IEnumerable<MetadataRouteParameterDefinition> metadataRouteParameterDefinitions)
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
            Required = true,
            In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.ParameterDefinitionType),
        });
    }

    private static IEnumerable<OpenApiParameter> GetOpenApiParametersForMapFrom(MetadataMapFromDefinition metadataRouteParameterDefinitions)
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
                                                    Required = true,
                                                    In = TransformMetadataRouteParameterDefinitionTypeToOpenApiParameterLocation(parameterDefinition.MapFromAttribute.MapFrom),
                                                });
    }
}