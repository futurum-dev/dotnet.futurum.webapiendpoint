using Futurum.Core.Functional;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationFilter : IOperationFilter
{
    private readonly IWebApiEndpointMetadataCache _metadataCache;

    public WebApiEndpointOpenApiOperationFilter(IWebApiEndpointMetadataCache metadataCache)
    {
        _metadataCache = metadataCache;
    }

    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        if (operationFilterContext.ApiDescription.HttpMethod == MetadataRouteHttpMethod.Get.ToString() && openApiOperation.RequestBody != null)
        {
            openApiOperation.RequestBody.Required = false;
        }

        _metadataCache.Get(new WebApiEndpointMetadataCacheKey(operationFilterContext.ApiDescription.HttpMethod, operationFilterContext.ApiDescription.RelativePath))
                      .DoSwitch(metadataDefinition =>
                                {
                                    var openApiParameters = GetOpenApiParametersFromRoute(metadataDefinition);

                                    foreach (var openApiParameter in openApiParameters)
                                    {
                                        openApiOperation.Parameters.Add(openApiParameter);
                                    }

                                    UpdateOpenApiOperationInformation(openApiOperation, metadataDefinition);
                                },
                                Function.DoNothing);
    }

    private static IEnumerable<OpenApiParameter> GetOpenApiParametersFromRoute(MetadataDefinition metadataDefinition) =>
        metadataDefinition.MetadataRouteDefinition.ParameterDefinitions
                          .Select(parameterDefinition => new OpenApiParameter
                          {
                              Name = parameterDefinition.Name,
                              Schema = MapDotnetTypesToOpenApiTypes(parameterDefinition.Type),
                              Required = true,
                              In = ParameterLocation.Path
                          });

    private static void UpdateOpenApiOperationInformation(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        openApiOperation.Summary = metadataDefinition.MetadataRouteDefinition.OpenApiOperation?.Summary;
        openApiOperation.Description = metadataDefinition.MetadataRouteDefinition.OpenApiOperation?.Description;
    }

    private static OpenApiSchema MapDotnetTypesToOpenApiTypes(Type parameterType)
    {
        if (parameterType == typeof(int))
        {
            return new OpenApiSchema { Type = "integer", Format = "int32" };
        }

        if (parameterType == typeof(long))
        {
            return new OpenApiSchema { Type = "integer", Format = "int64" };
        }

        if (parameterType == typeof(float))
        {
            return new OpenApiSchema { Type = "number", Format = "float" };
        }

        if (parameterType == typeof(double))
        {
            return new OpenApiSchema { Type = "number", Format = "double" };
        }

        return null;
    }
}