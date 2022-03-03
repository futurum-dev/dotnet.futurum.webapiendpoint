using Futurum.Core.Functional;
using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class RequestUploadFileWithPayloadOpenApiRequestConfiguration : IWebApiOpenApiRequestConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var requestDtoType = metadataDefinition.MetadataTypeDefinition.RequestDtoType;

        return requestDtoType.IsGenericType &&
               requestDtoType.GetGenericTypeDefinition() == typeof(RequestUploadFileWithPayloadDto<>);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        routeHandlerBuilder.Accepts(metadataDefinition.MetadataTypeDefinition.RequestDtoType, "multipart/form-data");
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        void Execute(KeyValuePair<string, OpenApiMediaType> content)
        {
            var multipartParameterDefinitions = metadataDefinition.MetadataMapFromMultipartDefinition.MapFromMultipartParameterDefinitions;
            content.Value.Schema = new OpenApiSchema
            {
                Type = "object",
                Required = new HashSet<string>(multipartParameterDefinitions.Select(x => x.Name)),
                Properties = multipartParameterDefinitions.Select(mapFromMultipartParameterDefinition =>
                                                          {
                                                              var (name, propertyInfo, _) = mapFromMultipartParameterDefinition;
                                                              
                                                              var openApiSchema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(propertyInfo.PropertyType);
        
                                                              return (Name: name, OpenApiSchema: openApiSchema);
                                                          })
                                                          .ToDictionary(x => x.Name, x => x.OpenApiSchema)
            };
        }
        
        openApiOperation.RequestBody.Content.TrySingle(x => x.Key == "multipart/form-data")
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }
}