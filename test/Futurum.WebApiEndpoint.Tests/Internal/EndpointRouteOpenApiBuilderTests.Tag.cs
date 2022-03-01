using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class EndpointRouteOpenApiBuilderTagTests
{
    [Fact]
    public void check()
    {
        var metadataTypeDefinition = new MetadataTypeDefinition(null, null, null, null, typeof(CommandApiEndpoint), null, null, null, null);

        var endpoint = TestRunner(metadataTypeDefinition, null, null);

        endpoint.Metadata.GetMetadata<TagsAttribute>().Tags.Single().Should().Be(typeof(CommandApiEndpoint).GetSanitizedLastPartOfNamespace());
    }

    private static RouteEndpoint TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                            MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<EndpointRouteOpenApiBuilderTag>();
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilderTag>();
        endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, metadataDefinition);

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
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

    private class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>, IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request().ToResultOkAsync();

        public ResponseDto Map(Response domain) =>
            new();
    }
}