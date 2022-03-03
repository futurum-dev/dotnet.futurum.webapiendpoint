using System.Net.Mime;

using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class ResponseAsyncEnumerableOpenApiResponseConfiguration : IWebApiOpenApiResponseConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var responseDtoType = metadataDefinition.MetadataTypeDefinition.ResponseDtoType;
        
        return responseDtoType.IsGenericType &&
               responseDtoType.GetGenericTypeDefinition() == typeof(ResponseAsyncEnumerableDto<>);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, _, _, _) = metadataDefinition;

        var responseDtoType = metadataDefinition.MetadataTypeDefinition.UnderlyingResponseDtoType;

        var type = typeof(IEnumerable<>).MakeGenericType(responseDtoType);

        routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, type, MediaTypeNames.Application.Json);
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
    }
}