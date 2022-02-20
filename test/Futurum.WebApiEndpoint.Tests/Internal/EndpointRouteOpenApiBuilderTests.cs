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

namespace Futurum.WebApiEndpoint.Tests.Internal;

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
                                                                    typeof(ICommandWebApiEndpoint<RequestPlainTextDto, ResponseDto, Request, Response, RequestPlainTextDtoMapper,
                                                                        RequestPlainTextDtoMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestPlainTextDto, ResponseDto, Request, Response, RequestPlainTextDtoMapper,
                                                                        RequestPlainTextDtoMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<EmptyRequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Text.Plain);
        }

        private class RequestPlainTextDtoMapper : IWebApiEndpointRequestMapper<RequestPlainTextDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestPlainTextDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseDto> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void RequestUploadFilesDto()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFilesDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestUploadFilesDto, ResponseDto, Request, Response, RequestUploadFilesDtoMapper,
                                                                        RequestUploadFilesDtoMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestUploadFilesDto, ResponseDto, Request, Response, RequestUploadFilesDtoMapper,
                                                                        RequestUploadFilesDtoMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestUploadFilesDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be("multipart/form-data");
        }

        public class RequestUploadFilesDtoMapper : IWebApiEndpointRequestMapper<RequestUploadFilesDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestUploadFilesDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseDto> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void RequestDto_without_MapFrom()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, RequestDto_without_MapFromMapper,
                                                                        RequestDto_without_MapFromMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, RequestDto_without_MapFromMapper,
                                                                        RequestDto_without_MapFromMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());

            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        public class RequestDto_without_MapFromMapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseDto> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void RequestDto_with_MapFrom()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, RequestDto_with_MapFromMapper,
                                                                        RequestDto_with_MapFromMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, RequestDto_with_MapFromMapper,
                                                                        RequestDto_with_MapFromMapper>),
                                                                    new List<Type>());

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

        public class RequestDto_with_MapFromMapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseDto> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void RequestDto_with_MapFromMultipart()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, RequestDto_with_MapFromMapper,
                                                                        RequestDto_with_MapFromMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, RequestDto_with_MapFromMapper,
                                                                        RequestDto_with_MapFromMapper>),
                                                                    new List<Type>());

            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

            var metadataMapFromMultipartParameterDefinition = new MetadataMapFromMultipartParameterDefinition("Id",
                                                                                            typeof(RequestDto).GetProperties().Single(x => x.Name == nameof(RequestDto.Id)),
                                                                                            new MapFromMultipartJsonAttribute(1));
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>{metadataMapFromMultipartParameterDefinition});
            
            var (serviceProvider, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be("multipart/form-data");
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
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(TestRequestStreamDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, TestRequestStreamDto, Request, Response, IResponseStreamDtoMapper,
                                                                        IResponseStreamDtoMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, TestRequestStreamDto, Request, Response, IResponseStreamDtoMapper,
                                                                        IResponseStreamDtoMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
            
            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var successProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
            successProducesResponseTypeMetadata.Type.Should().Be(typeof(void));
            successProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            successProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Octet);

            var failedProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Skip(1).First();
            failedProducesResponseTypeMetadata.Type.Should().Be(typeof(void));
            failedProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);
            failedProducesResponseTypeMetadata.ContentTypes.Should().BeEmpty();
        }

        public class IResponseStreamDtoMapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, TestRequestStreamDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();

            public Result<TestRequestStreamDto> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void ResponseAsyncEnumerableDto()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseAsyncEnumerableDto<int>), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseAsyncEnumerableDto<int>, Request, Response, ResponseAsyncEnumerableDtoMapper,
                                                                        ResponseAsyncEnumerableDtoMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseAsyncEnumerableDto<int>, Request, Response,
                                                                        ResponseAsyncEnumerableDtoMapper, ResponseAsyncEnumerableDtoMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
            
            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var successProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
            successProducesResponseTypeMetadata.Type.Should().Be<IEnumerable<int>>();
            successProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            successProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);

            var failedProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Skip(1).First();
            failedProducesResponseTypeMetadata.Type.Should().Be(typeof(void));
            failedProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);
            failedProducesResponseTypeMetadata.ContentTypes.Should().BeEmpty();
        }

        public class ResponseAsyncEnumerableDtoMapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseAsyncEnumerableDto<int>>
        {
            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseAsyncEnumerableDto<int>> Map(Response domain) =>
                throw new NotImplementedException();
        }

        [Fact]
        public void NotEmptyResponseDto()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, NotEmptyResponseDtoMapper, NotEmptyResponseDtoMapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, NotEmptyResponseDtoMapper,
                                                                        NotEmptyResponseDtoMapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
            
            var (_, endpoint) = TestRunner(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var successProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
            successProducesResponseTypeMetadata.Type.Should().Be<ResponseDto>();
            successProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            successProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);

            var failedProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Skip(1).First();
            failedProducesResponseTypeMetadata.Type.Should().Be(typeof(void));
            failedProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);
            failedProducesResponseTypeMetadata.ContentTypes.Should().BeEmpty();
        }

        public class NotEmptyResponseDtoMapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
        {
            public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
                throw new NotImplementedException();

            public Result<ResponseDto> Map(Response domain) =>
                throw new NotImplementedException();
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
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestDto), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                    typeof(ICommandWebApiEndpoint<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                    typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                    typeof(CommandWebApiEndpointDispatcher<RequestDto, ResponseDto, Request, Response, Mapper, Mapper>),
                                                                    new List<Type>());
            var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
            
            var (_, endpoint) = TestRunner(metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            endpoint.Metadata.GetMetadata<TagsAttribute>().Tags.Single().Should().Be(typeof(CommandApiEndpoint).GetSanitizedLastPartOfNamespace());
        }

        private static (IServiceProvider serviceProvider, RouteEndpoint routeEndpoint) TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition, 
                                                                                                  MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

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

    private class CommandApiEndpoint : CommandWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new Response().ToResultOkAsync();
    }

    private class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request().ToResultOk();

        public Result<ResponseDto> Map(Response domain) =>
            new ResponseDto().ToResultOk();
    }
}