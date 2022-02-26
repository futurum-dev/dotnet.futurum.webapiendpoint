using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Internal.Dispatcher;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class EndpointRouteBuilderExtensionsTests
{
    private readonly ITestOutputHelper _output;

    public EndpointRouteBuilderExtensionsTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void correct_Route_is_set()
    {
        var route = "test-route-123";

        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, route, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton<IWebApiEndpointMetadataCache>(new TestWebApiEndpointMetadataCache(metadataDefinition));
            serviceCollection.AddSingleton<IEndpointRouteOpenApiBuilder>(new TestEndpointRouteOpenApiBuilder());
            serviceCollection.AddSingleton<IEndpointRouteSecurityBuilder>(new TestEndpointRouteSecurityBuilder());
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        application.UseWebApiEndpoints();

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

        endpoint.RoutePattern.RawText.Should().Be(WebApiEndpointConfiguration.Default.RouteFactory(WebApiEndpointConfiguration.Default, metadataDefinition.MetadataRouteDefinition));
    }

    [Fact]
    public void check_MetadataDefinition_is_set_in_Metadata()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route-123", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton<IWebApiEndpointMetadataCache>(new TestWebApiEndpointMetadataCache(metadataDefinition));
            serviceCollection.AddSingleton<IEndpointRouteOpenApiBuilder>(new TestEndpointRouteOpenApiBuilder());
            serviceCollection.AddSingleton<IEndpointRouteSecurityBuilder>(new TestEndpointRouteSecurityBuilder());
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        application.UseWebApiEndpoints();

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

        endpoint.Metadata.GetMetadata<MetadataDefinition>().Should().Be(metadataDefinition);
    }

    [Fact]
    public void check_other_EndpointRoute_Builders_are_called()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route-123", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var testEndpointRouteOpenApiBuilder = new TestEndpointRouteOpenApiBuilder();
        var testEndpointRouteSecurityBuilder = new TestEndpointRouteSecurityBuilder();

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton<IWebApiEndpointMetadataCache>(new TestWebApiEndpointMetadataCache(metadataDefinition));
            serviceCollection.AddSingleton<IEndpointRouteOpenApiBuilder>(testEndpointRouteOpenApiBuilder);
            serviceCollection.AddSingleton<IEndpointRouteSecurityBuilder>(testEndpointRouteSecurityBuilder);
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        application.UseWebApiEndpoints();

        testEndpointRouteOpenApiBuilder.WasCalled.Should().BeTrue();
        testEndpointRouteSecurityBuilder.WasCalled.Should().BeTrue();
    }
    
    [Fact]
    public void check_MetadataRouteDefinition_ExtendedOptions_is_called()
    {
        var wasCalled = false;
        
        var extendedOptionsAction = (RouteHandlerBuilder routeHandlerBuilder) => { wasCalled = true;};
        
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route-123", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  extendedOptionsAction, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);


        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton<IWebApiEndpointMetadataCache>(new TestWebApiEndpointMetadataCache(metadataDefinition));
            serviceCollection.AddSingleton<IEndpointRouteOpenApiBuilder>(new TestEndpointRouteOpenApiBuilder());
            serviceCollection.AddSingleton<IEndpointRouteSecurityBuilder>(new TestEndpointRouteSecurityBuilder());
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        application.UseWebApiEndpoints();

        wasCalled.Should().BeTrue();
    }

    public class HttpMethod
    {
        [Fact]
        public void HttpMethod_Get()
        {
            TestRunner(MetadataRouteHttpMethod.Get);
        }

        [Fact]
        public void HttpMethod_Put()
        {
            TestRunner(MetadataRouteHttpMethod.Put);
        }

        [Fact]
        public void HttpMethod_Post()
        {
            TestRunner(MetadataRouteHttpMethod.Post);
        }

        [Fact]
        public void HttpMethod_Patch()
        {
            TestRunner(MetadataRouteHttpMethod.Patch);
        }

        [Fact]
        public void HttpMethod_Delete()
        {
            TestRunner(MetadataRouteHttpMethod.Delete);
        }

        private static void TestRunner(MetadataRouteHttpMethod httpMethod)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(httpMethod, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IWebApiEndpointMetadataCache>(new TestWebApiEndpointMetadataCache(metadataDefinition));
                serviceCollection.AddSingleton<IEndpointRouteOpenApiBuilder>(new TestEndpointRouteOpenApiBuilder());
                serviceCollection.AddSingleton<IEndpointRouteSecurityBuilder>(new TestEndpointRouteSecurityBuilder());
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
            });

            var application = builder.Build();

            application.UseWebApiEndpoints();

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

            endpoint.Metadata.GetMetadata<HttpMethodMetadata>().HttpMethods.Single().Should().Be(metadataDefinition.MetadataRouteDefinition.HttpMethod.ToString().ToUpper());
        }
    }

    private class TestEndpointRouteOpenApiBuilder : IEndpointRouteOpenApiBuilder
    {
        public bool WasCalled { get; private set; }

        public void Configure(RouteHandlerBuilder routeHandlerBuilder, MetadataDefinition metadataDefinition)
        {
            WasCalled = true;
        }
    }

    private class TestEndpointRouteSecurityBuilder : IEndpointRouteSecurityBuilder
    {
        public bool WasCalled { get; private set; }

        public void Configure(RouteHandlerBuilder routeHandlerBuilder, WebApiEndpointConfiguration webApiEndpointConfiguration, MetadataDefinition metadataDefinition)
        {
            WasCalled = true;
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

    public record RequestDto;

    public record Request;

    public record ResponseDto;

    public record Response;

    private class CommandApiEndpoint : CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new Response().ToResultOkAsync();
    }

    private class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request().ToResultOk();

        public ResponseDto Map(HttpContext httpContext, Response domain) => 
            new();
    }
}