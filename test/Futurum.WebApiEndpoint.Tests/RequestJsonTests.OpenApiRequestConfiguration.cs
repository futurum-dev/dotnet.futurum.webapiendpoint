using System.Net.Mime;
using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;
using Futurum.WebApiEndpoint.Tests.OpenApi;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestJsonOpenApiRequestConfigurationTests
{
    public class EndpointRouteBuilder
    {
        public class RequestDto_without_MapFrom
        {
            [Fact]
            public void check()
            {
                var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(RequestDto), null, null, null, null, null, null, null, null);

                var (serviceProvider, endpoint) = TestRunner(metadataTypeDefinition, null, null);

                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
            }
        }

        public class RequestDto_with_MapFrom
        {
            [Fact]
            public void check()
            {
                var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(RequestDto), null, null, null, null, null, null, null, null);

                var metadataMapFromParameterDefinition = new MetadataMapFromParameterDefinition("Id",
                                                                                                typeof(RequestDto).GetProperties().Single(x => x.Name == nameof(RequestDto.Id)),
                                                                                                new MapFromPathAttribute("Id"));
                var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition> { metadataMapFromParameterDefinition });

                var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

                var (serviceProvider, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

                var requestOpenApiTypeCreator = serviceProvider.GetService(typeof(IRequestOpenApiTypeCreator)) as IRequestOpenApiTypeCreator;

                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be(requestOpenApiTypeCreator.Get(typeof(RequestDto)));
                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
            }
        }

        public class RequestDto_with_MapFromMultipart
        {
            [Fact]
            public void check()
            {
                var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(RequestDto), null, null, null, null, null, null, null, null);

                var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

                var metadataMapFromMultipartParameterDefinition = new MetadataMapFromMultipartParameterDefinition("Id",
                                                                                                                  typeof(RequestDto).GetProperties().Single(x => x.Name == nameof(RequestDto.Id)),
                                                                                                                  new MapFromMultipartJsonAttribute(1));
                var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition> { metadataMapFromMultipartParameterDefinition });

                var (serviceProvider, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
                endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be("multipart/form-data");
            }
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                                                                  MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            return TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition,
                                                                                                  MetadataMapFromDefinition metadataMapFromDefinition,
                                                                                                  MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
                serviceCollection.AddSingleton<RequestJsonOpenApiRequestConfiguration>();
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiRequestConfiguration = endpointRouteBuilder.ServiceProvider.GetService<RequestJsonOpenApiRequestConfiguration>();
            openApiRequestConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

            return (endpointRouteBuilder.ServiceProvider, endpoint);
        }

        public record RequestDto(string FirstName)
        {
            [MapFromPath("Id")] public string Id { get; set; }
        }
    }

    public class Swashbuckle
    {
        public class when_MapFrom
        {
            [Fact]
            public void check()
            {
                var routeTemplate = "test-route";
                var httpMethod = MetadataRouteHttpMethod.Post;
                var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, routeTemplate, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                          Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

                var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<CommandDto>), typeof(CommandDto), typeof(ResponseEmptyDto), typeof(ResponseEmptyDto), typeof(ApiEndpoint),
                                                                        typeof(ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseEmptyDto, Command, ResponseEmpty,
                                                                            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseEmptyMapper>),
                                                                        typeof(IWebApiEndpointMiddlewareExecutor<Command, ResponseEmpty>),
                                                                        typeof(WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseEmptyDto, Command, ResponseEmpty,
                                                                            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseEmptyMapper>),
                                                                        new List<Type>());
                var metadataMapFromDefinition = new MetadataMapFromDefinition(WebApiEndpointMetadataTypeService.GetMapFromProperties(typeof(CommandDto))
                                                                                                               .Select(x => new MetadataMapFromParameterDefinition(
                                                                                                                           x.propertyInfo.Name, x.propertyInfo, x.mapFromAttribute))
                                                                                                               .ToList());
                var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, null);

                var openApiRequestConfiguration = new RequestJsonOpenApiRequestConfiguration(new RequestOpenApiTypeCreator());

                var openApiOperation = new OpenApiOperation();
                var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                        {
                                                                            HttpMethod = httpMethod.ToString(),
                                                                            RelativePath = routeTemplate
                                                                        },
                                                                        new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                            new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                        new SchemaRepository(null),
                                                                        typeof(WebApiEndpointOpenApiOperationTypeInformationTests).GetMethods()[0]);
                openApiRequestConfiguration.Execute(openApiOperation, operationFilterContext, metadataDefinition);

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

            public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.NoResponse.Mapper<Mapper>
            {
                public override Task<Result<ResponseEmpty>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
                    ResponseEmpty.Default.ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
            {
                public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
                    new Command(dto.Id).ToResultOkAsync();
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
                                                                          new List<MetadataRouteParameterDefinition> { new("Id", MetadataRouteParameterDefinitionType.Path, typeof(string)) }, null,
                                                                          200,
                                                                          400,
                                                                          Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

                var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<CommandDto>), typeof(CommandDto), typeof(ResponseEmptyDto), typeof(ResponseEmptyDto), typeof(ApiEndpoint),
                                                                        typeof(ICommandWebApiEndpoint<RequestJsonDto<CommandDto>, ResponseEmptyDto, Command, ResponseEmpty,
                                                                            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseEmptyMapper>),
                                                                        typeof(IWebApiEndpointMiddlewareExecutor<Command, ResponseEmpty>),
                                                                        typeof(WebApiEndpointDispatcher<RequestJsonDto<CommandDto>, ResponseEmptyDto, Command, ResponseEmpty,
                                                                            RequestJsonMapper<CommandDto, Command, Mapper>, ResponseEmptyMapper>),
                                                                        new List<Type>());

                var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

                var openApiRequestConfiguration = new RequestJsonOpenApiRequestConfiguration(new RequestOpenApiTypeCreator());

                var openApiOperation = new OpenApiOperation();
                var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                        {
                                                                            HttpMethod = httpMethod.ToString(),
                                                                            RelativePath = routeTemplate
                                                                        },
                                                                        new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                            new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                        new SchemaRepository(null),
                                                                        typeof(WebApiEndpointOpenApiOperationTypeInformationTests).GetMethods()[0]);
                openApiRequestConfiguration.Execute(openApiOperation, operationFilterContext, metadataDefinition);

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

            public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.NoResponse.Mapper<Mapper>
            {
                public override Task<Result<ResponseEmpty>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
                    ResponseEmpty.Default.ToResultOkAsync();
            }

            public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
            {
                public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
                    new Command(dto.Id).ToResultOkAsync();
            }
        }
    }
}