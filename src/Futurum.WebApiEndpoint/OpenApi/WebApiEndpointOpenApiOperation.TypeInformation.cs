using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationTypeInformation : IOperationFilter
{
    private readonly IWebApiEndpointMetadataCache _metadataCache;
    private readonly IEnumerable<IWebApiOpenApiRequestConfiguration> _openApiRequestsConfigurations;
    private readonly IEnumerable<IWebApiOpenApiResponseConfiguration> _openApiResponseConfigurations;

    public WebApiEndpointOpenApiOperationTypeInformation(IWebApiEndpointMetadataCache metadataCache,
                                                         IEnumerable<IWebApiOpenApiRequestConfiguration> openApiRequestsConfigurations,
                                                         IEnumerable<IWebApiOpenApiResponseConfiguration> openApiResponseConfigurations)
    {
        _metadataCache = metadataCache;
        _openApiRequestsConfigurations = openApiRequestsConfigurations;
        _openApiResponseConfigurations = openApiResponseConfigurations;
    }

    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        _metadataCache.Get(new WebApiEndpointMetadataCacheKey(operationFilterContext.ApiDescription.HttpMethod, operationFilterContext.ApiDescription.RelativePath))
                      .DoSwitch(metadataDefinition => UpdateOpenApiOperation(openApiOperation, operationFilterContext, metadataDefinition),
                                Function.DoNothing);
    }

    private void UpdateOpenApiOperation(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        foreach (var openApiRequestsConfiguration in _openApiRequestsConfigurations)
        {
            if (openApiRequestsConfiguration.Check(metadataDefinition))
            {
                openApiRequestsConfiguration.Execute(openApiOperation, operationFilterContext, metadataDefinition);
            }
        }
        
        foreach (var openApiResponseConfiguration in _openApiResponseConfigurations)
        {
            if (openApiResponseConfiguration.Check(metadataDefinition))
            {
                openApiResponseConfiguration.Execute(openApiOperation, operationFilterContext, metadataDefinition);
            }
        }
    }
}