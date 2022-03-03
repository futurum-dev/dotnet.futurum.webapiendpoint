using System.Net.Mime;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseDataCollectionOpenApiRequestConfigurationTests
{
    [Fact]
    public void check()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseDataCollectionDto<int>), typeof(int), null, null, null, null, null, null);

        var endpoint = TestRunner(metadataRouteDefinition, metadataTypeDefinition, null, null);

        var successProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
        successProducesResponseTypeMetadata.Type.Should().Be<ResponseDataCollectionDto<int>>();
        successProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
        successProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
    }

    private static RouteEndpoint TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                            MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
    {
        var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

        var builder = WebApplication.CreateBuilder();

        builder.Host.ConfigureServices((_, serviceCollection) =>
        {
            serviceCollection.AddSingleton<ResponseDataCollectionOpenApiResponseConfiguration>();
        });

        var application = builder.Build();

        var httpMethod = "GET";
        var route = string.Empty;

        var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

        var endpointRouteBuilder = application as IEndpointRouteBuilder;

        var openApiResponseConfiguration = endpointRouteBuilder.ServiceProvider.GetService<ResponseDataCollectionOpenApiResponseConfiguration>();
        openApiResponseConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

        var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

        return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
    }
}