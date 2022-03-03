using Futurum.Core.Functional;
using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class RequestUploadFilesOpenApiRequestConfiguration : IWebApiOpenApiRequestConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var requestDtoType = metadataDefinition.MetadataTypeDefinition.RequestDtoType;

        return requestDtoType == typeof(RequestUploadFilesDto);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        routeHandlerBuilder.Accepts(typeof(RequestUploadFilesDto), "multipart/form-data");
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string> { "files" },
                Properties = new Dictionary<string, OpenApiSchema>
                {
                    {
                        "files", WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(IEnumerable<IFormFile>))
                    }
                }
            };
        }

        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }
}