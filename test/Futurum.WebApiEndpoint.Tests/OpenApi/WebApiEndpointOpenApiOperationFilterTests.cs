using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Functional;
using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointOpenApiOperationFilterTests
{
    public class when_RequestUploadFilesDto
    {
        [Fact]
        public void check()
        {
            var routeTemplate = "test-route";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFilesDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>, RequestUploadFilesMapper<ApiEndpoint>>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<RequestUploadFiles<ApiEndpoint>, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>,
                                                                        RequestUploadFilesMapper<ApiEndpoint>>),
                                                                    new List<Type>());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

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
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.RequestBody.Content.Count.Should().Be(1);
            var openApiMediaType = openApiOperation.RequestBody.Content.Where(x => x.Key == "multipart/form-data")
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Schema.Type.Should().Be("object");
            openApiMediaType.Schema.Required.Should().BeEquivalentTo(EnumerableExtensions.Return(nameof(RequestUploadFilesDto.Files)));
            openApiMediaType.Schema.Properties.Count.Should().Be(1);
            openApiMediaType.Schema.Properties[nameof(RequestUploadFilesDto.Files)].Should()
                            .BeEquivalentTo(new OpenApiSchema { Type = "array", Items = new OpenApiSchema { Type = "string", Format = "binary" } });
        }

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithoutResponse
        {
            protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }
    }

    public class when_RequestUploadFileDto
    {
        [Fact]
        public void check()
        {
            var routeTemplate = "test-route";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFileDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestUploadFileDto, RequestUploadFile<ApiEndpoint>, RequestUploadFileMapper<ApiEndpoint>>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<RequestUploadFile<ApiEndpoint>, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestUploadFileDto, RequestUploadFile<ApiEndpoint>,
                                                                        RequestUploadFileMapper<ApiEndpoint>>),
                                                                    new List<Type>());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

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
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.RequestBody.Content.Count.Should().Be(1);
            var openApiMediaType = openApiOperation.RequestBody.Content.Where(x => x.Key == "multipart/form-data")
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Schema.Type.Should().Be("object");
            openApiMediaType.Schema.Required.Should().BeEquivalentTo(EnumerableExtensions.Return("file"));
            openApiMediaType.Schema.Properties.Count.Should().Be(1);
            openApiMediaType.Schema.Properties["file"].Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
        }

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithoutResponse
        {
            protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }
    }

    public class when_MapFromMultipart
    {
        [Fact]
        public void check()
        {
            var routeTemplate = "test-route";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>),
                                                                    new List<Type>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(WebApiEndpointMetadataTypeService.GetMapFromMultipartProperties(typeof(CommandDto))
                                                                                                                             .Select(x => new MetadataMapFromMultipartParameterDefinition(
                                                                                                                                 x.propertyInfo.Name, x.propertyInfo, x.mapFromMultipartAttribute))
                                                                                                                             .ToList());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, metadataMapFromMultipartDefinition);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

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
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.RequestBody.Content.Count.Should().Be(1);
            var openApiMediaType = openApiOperation.RequestBody.Content.Where(x => x.Key == "multipart/form-data")
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Schema.Type.Should().Be("object");
            openApiMediaType.Schema.Required.Should().BeEquivalentTo(nameof(CommandDto.File), nameof(CommandDto.Payload));
            openApiMediaType.Schema.Properties.Count.Should().Be(metadataMapFromMultipartDefinition.MapFromMultipartParameterDefinitions.Count);

            openApiMediaType.Schema.Properties[nameof(CommandDto.File)].Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
            openApiMediaType.Schema.Properties[nameof(CommandDto.Payload)].Should()
                            .BeEquivalentTo(new OpenApiSchema
                            {
                                Type = "object", Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    {
                                        nameof(PayloadDto.Id), new OpenApiSchema { Type = "string" }
                                    }
                                }
                            });
        }

        public record CommandDto
        {
            [MapFromMultipartFile(0)] public IFormFile File { get; set; }

            [MapFromMultipartJson(1)] public PayloadDto Payload { get; set; }
        }

        public record PayloadDto(string Id);

        public record Command(string Id, IFormFile Files);

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
        {
            protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                new Command(dto.Payload.Id, dto.File).ToResultOk();
        }
    }

    public class when_MapFrom
    {
        [Fact]
        public void check()
        {
            var routeTemplate = "test-route";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(WebApiEndpointMetadataTypeService.GetMapFromProperties(typeof(CommandDto))
                                                                                                           .Select(x => new MetadataMapFromParameterDefinition(
                                                                                                                       x.propertyInfo.Name, x.propertyInfo, x.mapFromAttribute))
                                                                                                           .ToList());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, null);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

            var openApiOperation = new OpenApiOperation();
            var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                    {
                                                                        HttpMethod = httpMethod.ToString(),
                                                                        RelativePath = routeTemplate
                                                                    },
                                                                    new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                        new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                    new SchemaRepository(null),
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.Parameters.Count.Should().Be(1);
            var openApiParameter = openApiOperation.Parameters.Single();
            openApiParameter.Name.Should().Be("Id");
            openApiParameter.Required.Should().Be(true);
            openApiParameter.In.Should().Be(ParameterLocation.Path);
            openApiParameter.Schema.Should().BeEquivalentTo(new OpenApiSchema { Type = "string" });
        }

        public record CommandDto
        {
            [MapFromPath("Id")] public string Id { get; set; }
        }

        public record Command(string Id);

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
        {
            protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                new Command(dto.Id).ToResultOk();
        }
    }

    public class when_ManualParameterDefinitions
    {
        [Fact]
        public void check()
        {
            var routeTemplate = "test-route/{Id}";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null,
                                                                      new List<MetadataRouteParameterDefinition> { new("Id", MetadataRouteParameterDefinitionType.Path, typeof(string)) }, null, 200,
                                                                      400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(CommandDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<CommandDto, Command, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Command, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<CommandDto, Command, Mapper>),
                                                                    new List<Type>());

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

            var openApiOperation = new OpenApiOperation();
            var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                    {
                                                                        HttpMethod = httpMethod.ToString(),
                                                                        RelativePath = routeTemplate
                                                                    },
                                                                    new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                        new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                    new SchemaRepository(null),
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.Parameters.Count.Should().Be(1);
            var openApiParameter = openApiOperation.Parameters.Single();
            openApiParameter.Name.Should().Be("Id");
            openApiParameter.Required.Should().Be(true);
            openApiParameter.In.Should().Be(ParameterLocation.Path);
            openApiParameter.Schema.Should().BeEquivalentTo(new OpenApiSchema { Type = "string" });
        }

        public record CommandDto
        {
            [MapFromPath("Id")] public string Id { get; set; }
        }

        public record Command(string Id);

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithoutResponse.WithMapper<Mapper>
        {
            protected override Task<Result> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }

        public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
        {
            public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
                new Command(dto.Id).ToResultOk();
        }
    }

    public class OpenApiOperationInformation
    {
        [Fact]
        public void check()
        {
            var summary = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();

            var routeTemplate = "test-route";
            var httpMethod = MetadataRouteHttpMethod.Post;
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(),
                                                                      new MetadataRouteOpenApiOperation(summary, description), 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFilesDto), typeof(EmptyResponseDto), typeof(ApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>, RequestUploadFilesMapper<ApiEndpoint>>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<RequestUploadFiles<ApiEndpoint>, Unit>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestUploadFilesDto, RequestUploadFiles<ApiEndpoint>,
                                                                        RequestUploadFilesMapper<ApiEndpoint>>),
                                                                    new List<Type>());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var webApiEndpointOpenApiOperationFilter = new WebApiEndpointOpenApiOperationFilter(new TestWebApiEndpointMetadataCache(metadataDefinition));

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
                                                                    typeof(WebApiEndpointOpenApiOperationFilterTests).GetMethods()[0]);
            webApiEndpointOpenApiOperationFilter.Apply(openApiOperation, operationFilterContext);

            openApiOperation.Summary.Should().Be(summary);
            openApiOperation.Description.Should().Be(description);
        }

        public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithoutResponse
        {
            protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
                Result.OkAsync();
        }
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