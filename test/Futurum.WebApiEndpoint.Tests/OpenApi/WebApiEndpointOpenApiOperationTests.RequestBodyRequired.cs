using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointOpenApiOperationRequestBodyRequiredTests
{
    [Fact]
    public void check()
    {
        var httpMethod = MetadataRouteHttpMethod.Get;
        
        var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationRequestBodyRequired();

        var openApiOperation = new OpenApiOperation
        {
            RequestBody = new OpenApiRequestBody
            {
                Required = true
            }
        };

        var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                {
                                                                    HttpMethod = httpMethod.ToString(),
                                                                    RelativePath = "test-route"
                                                                },
                                                                new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                    new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                new SchemaRepository(null),
                                                                typeof(WebApiEndpointOpenApiOperationRequestBodyRequiredTests).GetMethods()[0]);
        webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

        openApiOperation.RequestBody.Required.Should().Be(false);
    }
}