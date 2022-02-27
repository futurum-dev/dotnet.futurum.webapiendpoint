using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint.OpenApi;

internal class WebApiEndpointOpenApiOperationRequestBodyRequired : IOperationFilter
{
    public void Apply(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext)
    {
        if (operationFilterContext.ApiDescription.HttpMethod == MetadataRouteHttpMethod.Get.ToString() && openApiOperation.RequestBody != null)
        {
            openApiOperation.RequestBody.Required = false;
        }
    }
}