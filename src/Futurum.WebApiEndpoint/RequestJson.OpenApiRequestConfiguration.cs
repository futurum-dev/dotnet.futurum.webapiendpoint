using System.Net.Mime;

using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

internal class RequestJsonOpenApiRequestConfiguration : IWebApiOpenApiRequestConfiguration
{
    private readonly IRequestOpenApiTypeCreator _requestOpenApiTypeCreator;

    public RequestJsonOpenApiRequestConfiguration(IRequestOpenApiTypeCreator requestOpenApiTypeCreator)
    {
        _requestOpenApiTypeCreator = requestOpenApiTypeCreator;
    }

    public bool Check(MetadataDefinition metadataDefinition)
    {
        var requestDtoType = metadataDefinition.MetadataTypeDefinition.RequestDtoType;

        return requestDtoType.IsGenericType &&
               requestDtoType.GetGenericTypeDefinition() == typeof(RequestJsonDto<>);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (_, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition) = metadataDefinition;

        var requestDtoType = metadataTypeDefinition.UnderlyingRequestDtoType;

        if (metadataMapFromMultipartDefinition != null && metadataMapFromMultipartDefinition.MapFromMultipartParameterDefinitions.Any())
        {
            routeHandlerBuilder.Accepts(requestDtoType, "multipart/form-data");
        }
        else
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
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        OpenApiRequestParameterConfiguration.ConfigureParameters(openApiOperation, metadataDefinition);
    }
}