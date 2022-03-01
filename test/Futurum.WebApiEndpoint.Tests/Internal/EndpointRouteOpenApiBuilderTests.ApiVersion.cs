using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class EndpointRouteOpenApiBuilderApiVersionTests
{
    [Fact]
    public void check()
    {
        var apiVersion = new ApiVersion(2, 1);

        var endpoint = TestRunner(apiVersion);

        endpoint.Metadata.GetMetadata<EndpointGroupNameAttribute>().EndpointGroupName.Should().Be($"v{apiVersion}");
    }

    private static RouteEndpoint TestRunner(ApiVersion apiVersion)
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", apiVersion, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, null, null, null);

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
            serviceCollection.AddSingleton<EndpointRouteOpenApiBuilderApiVersion>();
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var endpointRouteSecurityBuilder = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilderApiVersion>();
        endpointRouteSecurityBuilder.Configure(routeHandlerBuilder, metadataDefinition);

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
    }
}