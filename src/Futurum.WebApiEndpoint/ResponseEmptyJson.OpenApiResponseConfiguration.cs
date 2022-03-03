using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class ResponseEmptyJsonOpenApiResponseConfiguration : IWebApiOpenApiResponseConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var responseDtoType = metadataDefinition.MetadataTypeDefinition.ResponseDtoType;

        return responseDtoType == typeof(ResponseEmptyJsonDto);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, _, _, _) = metadataDefinition;

        routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode);
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
    }
}