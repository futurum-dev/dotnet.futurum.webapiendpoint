using System.Text.Json;

using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Moq;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointOpenApiOperationTypeInformationTests
{
    [Fact]
    public void check()
    {
        var routeTemplate = "test-route";
        var httpMethod = MetadataRouteHttpMethod.Post;
        var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestEmptyDto), typeof(RequestEmptyDto), typeof(ResponseEmptyDto), typeof(ResponseEmptyDto), typeof(ApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<RequestEmpty, ResponseEmpty>),
                                                                typeof(
                                                                    WebApiEndpointDispatcher<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                new List<Type>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

        var mockWebApiOpenApiRequestConfiguration = new Mock<IWebApiOpenApiRequestConfiguration>();
        mockWebApiOpenApiRequestConfiguration.Setup(x => x.Check(metadataDefinition))
                                             .Returns(true);
        
        var mockWebApiOpenApiResponseConfiguration = new Mock<IWebApiOpenApiResponseConfiguration>();
        mockWebApiOpenApiResponseConfiguration.Setup(x => x.Check(metadataDefinition))
                                              .Returns(true);

        var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationTypeInformation(new TestWebApiEndpointMetadataCache(metadataDefinition),
                                                                                                     EnumerableExtensions.Return(mockWebApiOpenApiRequestConfiguration.Object),
                                                                                                     EnumerableExtensions.Return(mockWebApiOpenApiResponseConfiguration.Object));

        var openApiOperation = new OpenApiOperation
        {
            RequestBody = new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType> { { "multipart/form-data", new OpenApiMediaType() } }
            }
        };
        var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                {
                                                                    HttpMethod = httpMethod.ToString(),
                                                                    RelativePath = routeTemplate
                                                                },
                                                                new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                    new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                new SchemaRepository(null),
                                                                typeof(WebApiEndpointOpenApiOperationTypeInformationTests).GetMethods()[0]);
        webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

        mockWebApiOpenApiRequestConfiguration.Verify(x => x.Check(metadataDefinition), Times.Once);
        mockWebApiOpenApiRequestConfiguration.Verify(x => x.Execute(openApiOperation, operationFilterContext, metadataDefinition), Times.Once);
        
        mockWebApiOpenApiResponseConfiguration.Verify(x => x.Check(metadataDefinition), Times.Once);
        mockWebApiOpenApiResponseConfiguration.Verify(x => x.Execute(openApiOperation, operationFilterContext, metadataDefinition), Times.Once);
    }

    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.NoResponse
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }

    private class TestWebApiEndpointMetadataCache : IWebApiEndpointMetadataCache
    {
        private readonly Dictionary<WebApiEndpointMetadataCacheKey, MetadataDefinition> _cache = new();

        public TestWebApiEndpointMetadataCache(MetadataDefinition metadataDefinition)
        {
            _cache.Add(new WebApiEndpointMetadataCacheKey(metadataDefinition.MetadataRouteDefinition.HttpMethod.ToString(), metadataDefinition.MetadataRouteDefinition.RouteTemplate),
                       metadataDefinition);
        }

        public IEnumerable<KeyValuePair<WebApiEndpointMetadataCacheKey, MetadataDefinition>> GetAll() =>
            _cache;

        public Result<MetadataDefinition> Get(WebApiEndpointMetadataCacheKey key) =>
            _cache.TryGetValue(key, () => $"Unable to find WebApiEndpoint Metadata for Key '{key}'");
    }
}