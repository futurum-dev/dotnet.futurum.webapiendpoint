using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

namespace Futurum.WebApiEndpoint.OpenApi;

public static class OpenApiRequestParameterConfiguration
{
    public static void ConfigureParameters(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        if (metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions.Any())
        {
            var openApiParameters = OpenApiRequestParameterForManualParameterConfiguration.Execute(metadataDefinition.MetadataRouteDefinition.ManualParameterDefinitions);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }

        if (metadataDefinition.MetadataMapFromDefinition != null)
        {
            var openApiParameters = OpenApiRequestParameterForMapFromConfiguration.Execute(metadataDefinition.MetadataMapFromDefinition);

            foreach (var openApiParameter in openApiParameters)
            {
                openApiOperation.Parameters.Add(openApiParameter);
            }
        }
    }
}