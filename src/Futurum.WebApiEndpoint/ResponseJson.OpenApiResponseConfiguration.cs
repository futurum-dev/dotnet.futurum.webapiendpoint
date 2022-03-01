using System.Net.Mime;

using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class ResponseJsonOpenApiResponseConfiguration : IWebApiOpenApiResponseConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var responseDtoType = metadataDefinition.MetadataTypeDefinition.ResponseDtoType;

        return responseDtoType.IsGenericType &&
               responseDtoType.GetGenericTypeDefinition() == typeof(ResponseJsonDto<>);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, _, _, _) = metadataDefinition;

        var responseDtoType = metadataDefinition.MetadataTypeDefinition.UnderlyingResponseDtoType;

        routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, responseDtoType, MediaTypeNames.Application.Json);
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
    }
}