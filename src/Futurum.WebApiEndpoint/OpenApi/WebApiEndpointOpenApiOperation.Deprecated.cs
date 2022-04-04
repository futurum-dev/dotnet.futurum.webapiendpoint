using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationDeprecated : IOperationFilter
{
    private readonly IWebApiEndpointMetadataCache _metadataCache;
    private readonly WebApiEndpointConfiguration _configuration;
    private readonly IEnumerable<WebApiEndpointOpenApiVersion> _openApiVersions;

    public WebApiEndpointOpenApiOperationDeprecated(IWebApiEndpointMetadataCache metadataCache,
                                                    WebApiEndpointConfiguration configuration,
                                                    IEnumerable<WebApiEndpointOpenApiVersion> openApiVersions)
    {
        _metadataCache = metadataCache;
        _configuration = configuration;
        _openApiVersions = openApiVersions;
    }

    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        _metadataCache.Get(new WebApiEndpointMetadataCacheKey(operationFilterContext.ApiDescription.HttpMethod, operationFilterContext.ApiDescription.RelativePath))
                      .DoSwitch(metadataDefinition => UpdateOpenApiOperation(openApiOperation, metadataDefinition),
                                Function.DoNothing);
    }

    private void UpdateOpenApiOperation(OpenApiOperation openApiOperation, MetadataDefinition metadataDefinition)
    {
        var routeApiVersion = metadataDefinition.MetadataRouteDefinition.ApiVersion ?? _configuration.DefaultApiVersion;

        var openApiVersion = _openApiVersions.Single(x => x.ApiVersion == routeApiVersion);

        var versionDeprecated = openApiVersion.Deprecated;

        if (metadataDefinition.MetadataRouteDefinition.OpenApiOperation != null)
        {
            metadataDefinition.MetadataRouteDefinition.OpenApiOperation.Deprecated
                              .DoSwitch(routeDeprecated =>
                                        {
                                            if (routeDeprecated)
                                            {
                                                if (versionDeprecated)
                                                {
                                                    openApiOperation.Deprecated = true;
                                                }
                                                else
                                                {
                                                    openApiOperation.Deprecated = true;
                                                }
                                            }
                                            else
                                            {
                                                if (versionDeprecated)
                                                {
                                                    openApiOperation.Deprecated = false;
                                                }
                                                else
                                                {
                                                    openApiOperation.Deprecated = false;
                                                }
                                            }
                                        },
                                        () => { openApiOperation.Deprecated = versionDeprecated; });
        }
        else
        {
            openApiOperation.Deprecated = versionDeprecated;
        }
    }
}