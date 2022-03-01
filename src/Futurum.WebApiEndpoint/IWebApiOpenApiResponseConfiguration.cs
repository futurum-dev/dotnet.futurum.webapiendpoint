using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public interface IWebApiOpenApiResponseConfiguration
{
    bool Check(MetadataDefinition metadataDefinition);
    
    void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition);
    
    void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition);
}