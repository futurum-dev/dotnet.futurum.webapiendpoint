using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;
using Futurum.WebApiEndpoint.Middleware;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class EndpointRouteSecurityBuilderTests
{
    private readonly ITestOutputHelper _output;

    public EndpointRouteSecurityBuilderTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void when_route_MetadataSecurityDefinition_is_not_specified_and_Configuration_SecureByDefault_is_false_configures_AllowAnonymous()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Post, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<RequestDto>), typeof(RequestDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        
        var builder = WebApplication.CreateBuilder();

        var webApiEndpointConfiguration = WebApiEndpointConfiguration.Default;
        
        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton(webApiEndpointConfiguration);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteSecurityBuilder = new EndpointRouteSecurityBuilder();
        endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, webApiEndpointConfiguration, metadataDefinition);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

        endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>().Should().NotBeNull();
    }

    [Fact]
    public void when_route_MetadataSecurityDefinition_is_not_specified_and_Configuration_SecureByDefault_is_true_configures_Authorize_without_policy()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Post, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<RequestDto>), typeof(RequestDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        
        var builder = WebApplication.CreateBuilder();

        var webApiEndpointConfiguration = WebApiEndpointConfiguration.Default with { SecureByDefault = true };
        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton(webApiEndpointConfiguration);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteSecurityBuilder = new EndpointRouteSecurityBuilder();
        endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, webApiEndpointConfiguration, metadataDefinition);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

        var authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();
        authorizeAttribute.Should().NotBeNull();
        authorizeAttribute.Policy.Should().BeNullOrEmpty();
    }

    [Fact]
    public void when_route_MetadataSecurityDefinition_is_specified_and_Configuration_SecureByDefault_is_true_configures_Authorize_with_policy()
    {
        var securityDefinition = new MetadataSecurityDefinition(new List<MetadataSecurityPermissionDefinition>(),
                                                                new List<MetadataSecurityRoleDefinition>(),
                                                                new List<MetadataSecurityClaimDefinition>());
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Post, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, securityDefinition);

        var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestJsonDto<RequestDto>), typeof(RequestDto), typeof(ResponseJsonDto<ResponseDto>), typeof(ResponseDto), typeof(CommandApiEndpoint),
                                                                typeof(ICommandWebApiEndpoint<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                typeof(IWebApiEndpointMiddlewareExecutor<Request, Response>),
                                                                typeof(WebApiEndpointDispatcher<RequestJsonDto<RequestDto>, ResponseJsonDto<ResponseDto>, Request, Response, RequestJsonMapper<RequestDto, Request, Mapper>, ResponseJsonMapper<Response, ResponseDto, Mapper>>),
                                                                new List<Type>());
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(new List<MetadataMapFromMultipartParameterDefinition>());
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);
        
        var builder = WebApplication.CreateBuilder();

        var webApiEndpointConfiguration = WebApiEndpointConfiguration.Default;
        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton(webApiEndpointConfiguration);
            serviceCollection.AddSingleton<IRequestOpenApiTypeCreator>(new RequestOpenApiTypeCreator());
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteSecurityBuilder = new EndpointRouteSecurityBuilder();
        endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, webApiEndpointConfiguration, metadataDefinition);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        var endpoint = dataSource.Endpoints.OfType<RouteEndpoint>().Single();

        var authorizeAttribute = endpoint.Metadata.GetMetadata<AuthorizeAttribute>();
        authorizeAttribute.Should().NotBeNull();
        authorizeAttribute.Policy.Should().Be(AuthorizationExtensions.ToAuthorizationPolicy(metadataDefinition));
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

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            throw new NotImplementedException();

        public ResponseDto Map(Response domain) =>
            throw new NotImplementedException();
    }
}