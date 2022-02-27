using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Functional;
using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointOpenApiOperationInformationTests
{
    [Fact]
    public void check()
    {
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var externalDocsDescription = Guid.NewGuid().ToString();
        var externalDocsUrl = new Uri("http://www.google.com");

        var routeTemplate = "test-route";
        var httpMethod = MetadataRouteHttpMethod.Post;
        var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(),
                                                                  new MetadataRouteOpenApiOperation(summary, description, Option<bool>.None, 
                                                                                                    new MetadataRouteOpenApiExternalDocs(externalDocsDescription, externalDocsUrl)), 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFilesDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>, RequestUploadFilesMapper<ApiEndpoint>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<RequestUploadFiles<ApiEndpoint>, Unit>),
                                                                typeof(CommandWebApiEndpointDispatcher<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>,
                                                                    RequestUploadFilesMapper<ApiEndpoint>>),
                                                                new List<Type>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

        var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationInformation(new TestWebApiEndpointMetadataCache(metadataDefinition));

        var openApiOperation = new OpenApiOperation();
        var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                {
                                                                    HttpMethod = httpMethod.ToString(),
                                                                    RelativePath = routeTemplate
                                                                },
                                                                new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                    new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                new SchemaRepository(null),
                                                                typeof(WebApiEndpointOpenApiOperationInformationTests).GetMethods()[0]);
        webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

        openApiOperation.Summary.Should().Be(summary);
        openApiOperation.Description.Should().Be(description);
        openApiOperation.ExternalDocs.Description.Should().Be(externalDocsDescription);
        openApiOperation.ExternalDocs.Url.Should().Be(externalDocsUrl);
    }

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithoutResponse
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