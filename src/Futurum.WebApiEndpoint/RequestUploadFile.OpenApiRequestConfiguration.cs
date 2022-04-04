using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class RequestUploadFileOpenApiRequestConfiguration : IWebApiOpenApiRequestConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var requestDtoType = metadataDefinition.MetadataTypeDefinition.RequestDtoType;

        return requestDtoType == typeof(RequestUploadFileDto);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        routeHandlerBuilder.Accepts(typeof(RequestUploadFileDto), "multipart/form-data");
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { "file" },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    { "file", WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(IFormFile)) }
                }
            };
        }

        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }
}