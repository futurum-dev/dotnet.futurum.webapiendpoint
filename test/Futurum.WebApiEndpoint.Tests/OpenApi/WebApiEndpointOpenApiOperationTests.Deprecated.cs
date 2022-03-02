using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointOpenApiOperationDeprecatedTests
{
    [Theory]
    [InlineData(true, null, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, true)]
    [InlineData(false, null, false)]
    [InlineData(true, true, true)]
    [InlineData(false, false, false)]
    public void check(bool versionDeprecated, bool? routeDeprecated, bool result)
    {
        var apiVersion = new ApiVersion(1, 0);
        var routeTemplate = "test-route";
        var httpMethod = MetadataRouteHttpMethod.Post;
        var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, apiVersion, new List<MetadataRouteParameterDefinition>(),
                                                                  new MetadataRouteOpenApiOperation(string.Empty, string.Empty, routeDeprecated.ToOption(), null), 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestEmptyDto), typeof(RequestEmptyDto), typeof(ResponseEmptyDto), typeof(ResponseEmptyDto), typeof(ApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<RequestEmpty, ResponseEmpty>),
                                                                typeof(WebApiEndpointDispatcher<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                new List<Type>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

        var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationDeprecated(new TestWebApiEndpointMetadataCache(metadataDefinition),
                                                                                                WebApiEndpointConfiguration.Default,
                                                                                                new[]
                                                                                                {
                                                                                                    new WebApiEndpointOpenApiVersion(string.Empty, apiVersion, versionDeprecated)
                                                                                                });

        var openApiOperation = new OpenApiOperation();
        var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                {
                                                                    HttpMethod = httpMethod.ToString(),
                                                                    RelativePath = routeTemplate
                                                                },
                                                                new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                    new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                new SchemaRepository(null),
                                                                typeof(WebApiEndpointOpenApiOperationDeprecatedTests).GetMethods()[0]);
        webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

        openApiOperation.Deprecated.Should().Be(result);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void when_MetadataRouteOpenApiOperation_is_null(bool versionDeprecated, bool result)
    {
        var apiVersion = new ApiVersion(1, 0);
        var routeTemplate = "test-route";
        var httpMethod = MetadataRouteHttpMethod.Post;
        var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, apiVersion, new List<MetadataRouteParameterDefinition>(),
                                                                  null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestEmptyDto), typeof(RequestEmptyDto), typeof(ResponseEmptyDto), typeof(ResponseEmptyDto), typeof(ApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<RequestEmpty, ResponseEmpty>),
                                                                typeof(WebApiEndpointDispatcher<RequestEmptyDto, ResponseEmptyDto, RequestEmpty, ResponseEmpty, RequestEmptyMapper, ResponseEmptyMapper>),
                                                                new List<Type>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

        var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationDeprecated(new TestWebApiEndpointMetadataCache(metadataDefinition),
                                                                                                WebApiEndpointConfiguration.Default,
                                                                                                new[]
                                                                                                {
                                                                                                    new WebApiEndpointOpenApiVersion(string.Empty, apiVersion, versionDeprecated)
                                                                                                });

        var openApiOperation = new OpenApiOperation();
        var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                {
                                                                    HttpMethod = httpMethod.ToString(),
                                                                    RelativePath = routeTemplate
                                                                },
                                                                new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                    new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                new SchemaRepository(null),
                                                                typeof(WebApiEndpointOpenApiOperationDeprecatedTests).GetMethods()[0]);
        webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

        openApiOperation.Deprecated.Should().Be(result);
    }

    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles<ApiEndpoint>.NoResponse
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