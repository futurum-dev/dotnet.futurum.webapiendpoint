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

public class RequestPlainTextOpenApiRequestConfigurationTests
{
    public class EndpointRouteBuilder
    {
        [Fact]
        public void check()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestPlainTextDto), null, null, null, null, null, null, null, null, null);

            var endpoint = TestRunner(metadataTypeDefinition, null, null);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<object>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be(MediaTypeNames.Text.Plain);
        }

        private static RouteEndpoint TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) => { serviceCollection.AddSingleton<RequestPlainTextOpenApiRequestConfiguration>(); });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiRequestConfiguration = endpointRouteBuilder.ServiceProvider.GetService<RequestPlainTextOpenApiRequestConfiguration>();
            openApiRequestConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }
}