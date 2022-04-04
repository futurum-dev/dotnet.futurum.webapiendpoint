using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationInformation : IOperationFilter
{
    private readonly IWebApiEndpointMetadataCache _metadataCache;

    public WebApiEndpointOpenApiOperationInformation(IWebApiEndpointMetadataCache metadataCache)
    {
        _metadataCache = metadataCache;
    }

    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        _metadataCache.Get(new WebApiEndpointMetadataCacheKey(operationFilterContext.ApiDescription.HttpMethod, operationFilterContext.ApiDescription.RelativePath))
                      .DoSwitch(metadataDefinition => UpdateOpenApiOperation(openApiOperation, metadataDefinition),
                                Function.DoNothing);
    }
    
    private static void UpdateOpenApiOperation(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        var metadataRouteOpenApiOperation = metadataDefinition.MetadataRouteDefinition.OpenApiOperation;
        if (metadataRouteOpenApiOperation != null)
        {
            openApiOperation.Summary = metadataRouteOpenApiOperation.Summary;
            openApiOperation.Description = metadataRouteOpenApiOperation.Description;

            if (metadataRouteOpenApiOperation.ExternalDocs != null)
            {
                openApiOperation.ExternalDocs = new OpenApiExternalDocs
                {
                    Description = metadataRouteOpenApiOperation.ExternalDocs.Description
                };
                
                if (metadataRouteOpenApiOperation.ExternalDocs.Url != null)
                {
                    openApiOperation.ExternalDocs.Url = metadataRouteOpenApiOperation.ExternalDocs.Url;
                }
            }
        }
    }
}