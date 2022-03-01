using System.Net;
using System.Net.Mime;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IEndpointRouteOpenApiBuilder
{
    void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition);
}

internal class EndpointRouteOpenApiBuilder : IEndpointRouteOpenApiBuilder
{
    private readonly IEnumerable<IWebApiOpenApiRequestConfiguration> _openApiRequestsConfigurations;
    private readonly IEnumerable<IWebApiOpenApiResponseConfiguration> _openApiResponseConfigurations;
    private readonly IEndpointRouteOpenApiBuilderApiVersion _openApiBuilderApiVersion;
    private readonly IEndpointRouteOpenApiBuilderTag _openApiBuilderTag;

    public EndpointRouteOpenApiBuilder(IEnumerable<IWebApiOpenApiRequestConfiguration> openApiRequestsConfigurations,
                                       IEnumerable<IWebApiOpenApiResponseConfiguration> openApiResponseConfigurations,
                                       IEndpointRouteOpenApiBuilderApiVersion openApiBuilderApiVersion,
                                       IEndpointRouteOpenApiBuilderTag openApiBuilderTag)
    {
        _openApiRequestsConfigurations = openApiRequestsConfigurations;
        _openApiResponseConfigurations = openApiResponseConfigurations;
        _openApiBuilderApiVersion = openApiBuilderApiVersion;
        _openApiBuilderTag = openApiBuilderTag;
    }

    public void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        ConfigureAccepts(routeHandlerBuilder, metadataDefinition);
        
        ConfigureProduces(routeHandlerBuilder, metadataDefinition);
        
        _openApiBuilderApiVersion.Configure(routeHandlerBuilder, metadataDefinition);
        
        _openApiBuilderTag.Configure(routeHandlerBuilder, metadataDefinition);
    }

    private void ConfigureAccepts(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        foreach (var openApiRequestsConfiguration in _openApiRequestsConfigurations)
        {
            if (openApiRequestsConfiguration.Check(metadataDefinition))
            {
                openApiRequestsConfiguration.Execute(routeHandlerBuilder, metadataDefinition);
            }
        }
    }

    private void ConfigureProduces(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        foreach (var openApiResponseConfiguration in _openApiResponseConfigurations)
        {
            if (openApiResponseConfiguration.Check(metadataDefinition))
            {
                openApiResponseConfiguration.Execute(routeHandlerBuilder, metadataDefinition);
            }
        }
        
        routeHandlerBuilder.Produces(metadataDefinition.MetadataRouteDefinition.FailedStatusCode, typeof(ResultErrorStructure), MediaTypeNames.Application.Json);

        if (metadataDefinition.MetadataRouteDefinition.FailedStatusCode != (int)HttpStatusCode.InternalServerError)
        {
            routeHandlerBuilder.Produces((int)HttpStatusCode.InternalServerError, typeof(ResultErrorStructure), MediaTypeNames.Application.Json);
        }
    }
}