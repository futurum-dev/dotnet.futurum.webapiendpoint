using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Linq;
using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFileOpenApiRequestConfigurationTests
{
    public class EndpointRouteBuilder
    {
        [Fact]
        public void check()
        {
            var metadataTypeDefinition = new MetadataTypeDefinition(typeof(RequestUploadFileDto), null, null, null, null, null, null, null, null, null);

            var endpoint = TestRunner(metadataTypeDefinition, null, null);

            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().RequestType.Should().Be<RequestUploadFileDto>();
            endpoint.Metadata.OfType<IAcceptsMetadata>().Single().ContentTypes.Single().Should().Be("multipart/form-data");
        }

        private static RouteEndpoint TestRunner(MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) => { serviceCollection.AddSingleton<RequestUploadFileOpenApiRequestConfiguration>(); });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiRequestConfiguration = endpointRouteBuilder.ServiceProvider.GetService<RequestUploadFileOpenApiRequestConfiguration>();
            openApiRequestConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }

    public class Swashbuckle
    {
        [Fact]
        public void check()
        {
            var openApiRequestConfiguration = new RequestUploadFileOpenApiRequestConfiguration();
            
            var openApiOperation = new OpenApiOperation
            {
                RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType> { { "multipart/form-data", new OpenApiMediaType() } }
                }
            };
            var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                    {
                                                                    },
                                                                    new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                        new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                    new SchemaRepository(null),
                                                                    typeof(Swashbuckle).GetMethods()[0]);
            openApiRequestConfiguration.Execute(openApiOperation, operationFilterContext, new MetadataDefinition(null,null,null, null));

            openApiOperation.RequestBody.Content.Count.Should().Be(1);
            var openApiMediaType = openApiOperation.RequestBody.Content.Where(x => x.Key == "multipart/form-data")
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Schema.Type.Should().Be("object");
            openApiMediaType.Schema.Required.Should().BeEquivalentTo(EnumerableExtensions.Return("file"));
            openApiMediaType.Schema.Properties.Count.Should().Be(1);
            openApiMediaType.Schema.Properties["file"].Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
        }
    }
}