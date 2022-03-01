using System.Net;
using System.Net.Mime;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using Moq;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class EndpointRouteOpenApiBuilderTests
{
    public class OpenApiRequestsConfigurations
    {
        [Fact]
        public void check()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseAsyncEnumerableDto<int>), typeof(ResponseAsyncEnumerableDto<int>), null, null, null, null, null, null);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var mockWebApiOpenApiRequestConfiguration = new Mock<IWebApiOpenApiRequestConfiguration>();
            mockWebApiOpenApiRequestConfiguration.Setup(x => x.Check(metadataDefinition))
                                                 .Returns(true);
            
            var routeHandlerBuilder = TestRunner(metadataDefinition, mockWebApiOpenApiRequestConfiguration.Object);

            mockWebApiOpenApiRequestConfiguration.Verify(x => x.Check(metadataDefinition), Times.Once);
            mockWebApiOpenApiRequestConfiguration.Verify(x => x.Execute(routeHandlerBuilder, metadataDefinition), Times.Once);
        }

        private static RouteHandlerBuilder TestRunner(MetadataDefinition metadataDefinition, IWebApiOpenApiRequestConfiguration openApiRequestConfiguration)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderApiVersion>().Object);
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderTag>().Object);
                serviceCollection.AddSingleton(openApiRequestConfiguration);
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var endpointRouteOpenApiBuilder = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilder>();
            endpointRouteOpenApiBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            return routeHandlerBuilder;
        }
    }

    public class OpenApiResponseConfigurations
    {
        [Fact]
        public void check()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseAsyncEnumerableDto<int>), typeof(ResponseAsyncEnumerableDto<int>), null, null, null, null, null, null);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var mockWebApiOpenApiResponseConfiguration = new Mock<IWebApiOpenApiResponseConfiguration>();
            mockWebApiOpenApiResponseConfiguration.Setup(x => x.Check(metadataDefinition))
                                                  .Returns(true);
            
            var routeHandlerBuilder = TestRunner(metadataDefinition, mockWebApiOpenApiResponseConfiguration.Object);

            mockWebApiOpenApiResponseConfiguration.Verify(x => x.Check(metadataDefinition), Times.Once);
            mockWebApiOpenApiResponseConfiguration.Verify(x => x.Execute(routeHandlerBuilder, metadataDefinition), Times.Once);
        }

        private static RouteHandlerBuilder TestRunner(MetadataDefinition metadataDefinition, IWebApiOpenApiResponseConfiguration openApiResponseConfiguration)
        {
            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderApiVersion>().Object);
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderTag>().Object);
                serviceCollection.AddSingleton(openApiResponseConfiguration);
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var endpointRouteOpenApiBuilder = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilder>();
            endpointRouteOpenApiBuilder.Configure(routeHandlerBuilder, metadataDefinition);

            return routeHandlerBuilder;
        }
    }

    public class Response_FailedStatusCode
    {
        [Fact]
        public void check()
        {
            var failedStatusCode = 400;
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, failedStatusCode,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseAsyncEnumerableDto<int>), typeof(ResponseAsyncEnumerableDto<int>), null, null, null, null, null, null);

            var endpoint = TestRunner(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var failedProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
            failedProducesResponseTypeMetadata.Type.Should().Be(typeof(ResultErrorStructure));
            failedProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);
            failedProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        private static RouteEndpoint TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderApiVersion>().Object);
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderTag>().Object);
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiResponseConfiguration = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilder>();
            openApiResponseConfiguration.Configure(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }

    public class Response_InternalServerError
    {
        [Fact]
        public void check()
        {
            var failedStatusCode = 400;
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, failedStatusCode,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseAsyncEnumerableDto<int>), typeof(ResponseAsyncEnumerableDto<int>), null, null, null, null, null, null);

            var endpoint = TestRunner(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var failedProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().Skip(1).First();
            failedProducesResponseTypeMetadata.Type.Should().Be(typeof(ResultErrorStructure));
            failedProducesResponseTypeMetadata.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            failedProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Json);
        }

        private static RouteEndpoint TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton(WebApiEndpointConfiguration.Default);
                serviceCollection.AddSingleton<EndpointRouteOpenApiBuilder>();
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderApiVersion>().Object);
                serviceCollection.AddSingleton(new Mock<IEndpointRouteOpenApiBuilderTag>().Object);
            });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiResponseConfiguration = endpointRouteBuilder.ServiceProvider.GetService<EndpointRouteOpenApiBuilder>();
            openApiResponseConfiguration.Configure(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }
}