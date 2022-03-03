using System.Net.Mime;

using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class RequestPlainTextOpenApiRequestConfiguration : IWebApiOpenApiRequestConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var requestDtoType = metadataDefinition.MetadataTypeDefinition.RequestDtoType;

        return requestDtoType == typeof(RequestPlainTextDto);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        routeHandlerBuilder.Accepts(typeof(object), MediaTypeNames.Text.Plain);
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
    }
}