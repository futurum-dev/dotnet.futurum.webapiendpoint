using System.Net.Mime;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Futurum.WebApiEndpoint;

public class ResponseFileStreamOpenApiResponseConfiguration : IWebApiOpenApiResponseConfiguration
{
    public bool Check(MetadataDefinition metadataDefinition)
    {
        var responseDtoType = metadataDefinition.MetadataTypeDefinition.ResponseDtoType;

        return responseDtoType == typeof(ResponseFileStreamDto);
    }

    public void Execute(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
    {
        var (metadataRouteDefinition, _, _, _) = metadataDefinition;

        routeHandlerBuilder.Produces(metadataRouteDefinition.SuccessStatusCode, contentType: MediaTypeNames.Application.Octet);
    }

    public void Execute(OpenApiOperation openApiOperation, OperationFilterContext operationFilterContext, MetadataDefinition metadataDefinition)
    {
        void Execute(KeyValuePair<string,OpenApiResponse> openApiResponse)
        {
            openApiResponse.Value.Content = new Dictionary<string, OpenApiMediaType>
            {
                {
                    MediaTypeNames.Application.Octet, new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema { Type = "string", Format = "binary" }
                    }
                }
            };
        }
        
        openApiOperation.Responses.TrySingle(x => x.Key == metadataDefinition.MetadataRouteDefinition.SuccessStatusCode.ToString())
                        .Do(x => x.DoSwitch(Execute, Function.DoNothing));
    }
}