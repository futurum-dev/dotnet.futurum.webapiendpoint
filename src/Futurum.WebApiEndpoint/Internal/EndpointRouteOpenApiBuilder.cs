using System.Net.Mime;

using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Internal;

internal interface IEndpointRouteOpenApiBuilder
{
    void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition);
}

internal class EndpointRouteOpenApiBuilder : IEndpointRouteOpenApiBuilder
{
    private readonly IRequestOpenApiTypeCreator _requestOpenApiTypeCreator;
    private readonly WebApiEndpointConfiguration _configuration;

    public EndpointRouteOpenApiBuilder(IRequestOpenApiTypeCreator requestOpenApiTypeCreator,
                                       WebApiEndpointConfiguration configuration)
    {
        _requestOpenApiTypeCreator = requestOpenApiTypeCreator;
        _configuration = configuration;
    }

    public void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        ConfigureAccepts(routeHandlerBuilder, metadataDefinition);
        ConfigureProduces(routeHandlerBuilder, metadataDefinition);
        ConfigureApiVersion(routeHandlerBuilder, metadataDefinition);
        ConfigureTags(routeHandlerBuilder, metadataDefinition);
    }

    private void ConfigureAccepts(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition) = metadataDefinition;

        var requestDtoType = metadataTypeDefinition.RequestDtoType;

        if (requestDtoType == typeof(RequestPlainTextDto))
        {
            routeHandlerBuilder.Accepts(typeof(EmptyRequestDto), MediaTypeNames.Text.Plain);
        }
        else if (requestDtoType != typeof(EmptyRequestDto) && !metadataRouteDefinition.AllowFileUploads)
        {
            if (metadataMapFromDefinition != null && metadataMapFromDefinition.MapFromParameterDefinitions.Any())
            {
                var requestOpenApiType = _requestOpenApiTypeCreator.Create(metadataMapFromDefinition, requestDtoType);

                if (requestOpenApiType.GetProperties().Any())
                {
                    routeHandlerBuilder.Accepts(requestOpenApiType, MediaTypeNames.Application.Json);
                }
            }
            else
            {
                routeHandlerBuilder.Accepts(requestDtoType, MediaTypeNames.Application.Json);
            }
        }
        else if (requestDtoType != typeof(EmptyRequestDto) && metadataRouteDefinition.AllowFileUploads)
        {
            routeHandlerBuilder.Accepts(requestDtoType, "multipart/form-data");
        }
    }

    private static void ConfigureProduces(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, metadataTypeDefinition, _) = metadataDefinition;

        var requestDtoType = metadataTypeDefinition.RequestDtoType;
        var responseDtoType = metadataDefinition.MetadataTypeDefinition.ResponseDtoType;

        if (responseDtoType.GetInterfaces().Contains(typeof(IResponseStreamDto)))
        {
            routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, contentType: MediaTypeNames.Application.Octet);
        }
        else if (responseDtoType.IsGenericType && responseDtoType.GetGenericTypeDefinition() == typeof(ResponseAsyncEnumerableDto<>))
        {
            var type = typeof(IEnumerable<>).MakeGenericType(responseDtoType.GetGenericArguments()[0]);

            routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, type, MediaTypeNames.Application.Json);
        }
        else if (requestDtoType != typeof(EmptyResponseDto))
        {
            routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, responseDtoType, MediaTypeNames.Application.Json);
        }
    }

    private void ConfigureApiVersion(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var apiVersion = metadataDefinition.MetadataRouteDefinition.ApiVersion ?? _configuration.DefaultApiVersion;
        routeHandlerBuilder.WithGroupName($"v{apiVersion}");
    }

    private static void ConfigureTags(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (_, metadataTypeDefinition, _) = metadataDefinition;

        var apiEndpointType = metadataTypeDefinition.WebApiEndpointType;

        routeHandlerBuilder.WithTags(apiEndpointType.GetSanitizedLastPartOfNamespace());
    }
}