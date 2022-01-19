using System.Net.Mime;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class EndpointRouteOpenApiBuilderTests
{
    private readonly ITestOutputHelper _output;

    public EndpointRouteOpenApiBuilderTests(ITestOutputHelper output)
    {
        _output = output;
    }

    public class ConfigureAccepts
    {
        [Fact]
        public void RequestPlainTextDto()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestPlainTextDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestPlainTextDto, ResponseDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<EmptyRequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Text.Plain);
        }

        [Fact]
        public void RequestDto_without_MapFrom()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        [Fact]
        public void RequestDto_with_MapFrom()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>));

            var metadataMapFromParameterDefinition = new MetadataMapFromParameterDefinition("Id",
                                                                                            typeof(RequestDto).GetProperties().Single(x => x.Name == nameof(RequestDto.Id)),
                                                                                            new MapFromPathAttribute("Id"));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition> { metadataMapFromParameterDefinition });

            var (serviceProvider, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition);

            var requestOpenApiTypeCreator = serviceProvider.GetService(typeof(IRequestOpenApiTypeCreator)) as IRequestOpenApiTypeCreator;

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be(requestOpenApiTypeCreator.Get(typeof(RequestDto)));
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        [Fact]
        public void RequestDto_AllowFileUploads()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, true,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be("multipart/form-data");
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            return TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition,
                                                                                                  MetadataMapFromDefinition metadataMapFromDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService(typeof(EndpointRouteOpenApiBuilder)) as IEndpointRouteOpenApiBuilder;
            endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

            return (endpointRouteBuilder.ServiceProvider, endpoint);
        }
    }

    public class ConfigureProduces
    {
        [Fact]
        public void IResponseStreamDto()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(TestRequestStreamDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, TestRequestStreamDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, TestRequestStreamDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().Type.Should().Be(typeof(void));
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Octet);
        }

        [Fact]
        public void ResponseAsyncEnumerableDto()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseAsyncEnumerableDto<int>), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseAsyncEnumerableDto<int>, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseAsyncEnumerableDto<int>, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().Type.Should().Be<IEnumerable<int>>();
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        [Fact]
        public void NotEmptyResponseDto()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().Type.Should().Be<ResponseDto>();
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition,
                                                                                                  MetadataMapFromDefinition metadataMapFromDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService(typeof(EndpointRouteOpenApiBuilder)) as IEndpointRouteOpenApiBuilder;
            endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

            return (endpointRouteBuilder.ServiceProvider, endpoint);
        }

        public record TestRequestStreamDto : IResponseStreamDto;
    }

    public class ConfigureTags
    {
        [Fact]
        public void check_correct_tag_is_set()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestPlainTextDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDto, Request, Response>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestPlainTextDto, ResponseDto, Request, Response>));
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition);

            endpoint.Metadata.GetMetadata<TagsAttribute>().Tags.Single().Should().Be(typeof(CommandApiEndpoint).GetSanitizedLastPartOfNamespace());
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService(typeof(EndpointRouteOpenApiBuilder)) as IEndpointRouteOpenApiBuilder;
            endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

            return (endpointRouteBuilder.ServiceProvider, endpoint);
        }
    }

    public record RequestDto(string FirstName)
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request;

    public record ResponseDto;

    public record Response;

    private class CommandApiEndpoint : CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>
    {
        protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new Response().ToResultOkAsync();
    }
}