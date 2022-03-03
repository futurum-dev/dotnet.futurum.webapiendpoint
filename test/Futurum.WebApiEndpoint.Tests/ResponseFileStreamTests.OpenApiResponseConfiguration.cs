using System.Net.Mime;
using System.Text.Json;

using FluentAssertions;

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

public class ResponseFileStreamOpenApiResponseConfigurationTests
{
    public class EndpointRouteBuilder
    {
        [Fact]
        public void check()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, "test-route", null, new List<MetadataRouteParameterDefinition>(), null, 200, 400,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var metadataTypeDefinition = new MetadataTypeDefinition(null, null, typeof(ResponseFileStreamDto), typeof(ResponseFileStreamDto), null, null, null, null, null, null);

            var endpoint = TestRunner(metadataRouteDefinition, metadataTypeDefinition, null, null);

            var successProducesResponseTypeMetadata = endpoint.Metadata.OfType<IProducesResponseTypeMetadata>().First();
            successProducesResponseTypeMetadata.Type.Should().Be(typeof(void));
            successProducesResponseTypeMetadata.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
            successProducesResponseTypeMetadata.ContentTypes.Single().Should().Be(MediaTypeNames.Application.Octet);
        }

        private static RouteEndpoint TestRunner(MetadataRouteDefinition metadataRouteDefinition, MetadataTypeDefinition metadataTypeDefinition, MetadataMapFromDefinition metadataMapFromDefinition,
                                                MetadataMapFromMultipartDefinition metadataMapFromMultipartDefinition)
        {
            var metadataDefinition = new MetadataDefinition(metadataRouteDefinition, metadataTypeDefinition, metadataMapFromDefinition, metadataMapFromMultipartDefinition);

            var builder = WebApplication.CreateBuilder();

            builder.Host.ConfigureServices((_, serviceCollection) => { serviceCollection.AddSingleton<ResponseFileStreamOpenApiResponseConfiguration>(); });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiResponseConfiguration = endpointRouteBuilder.ServiceProvider.GetService<ResponseFileStreamOpenApiResponseConfiguration>();
            openApiResponseConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }

    public class Swashbuckle
    {
        [Fact]
        public void check()
        {
            var key = 400;
            
            var openApiRequestConfiguration = new ResponseFileStreamOpenApiResponseConfiguration();
            
            var openApiOperation = new OpenApiOperation
            {
                Responses = new OpenApiResponses
                {
                    {
                        key.ToString(), new OpenApiResponse()
                    }
                }
            };
            var operationFilterContext = new OperationFilterContext(new ApiDescription
                                                                    {
                                                                    },
                                                                    new SchemaGenerator(new SchemaGeneratorOptions(),
                                                                                        new JsonSerializerDataContractResolver(new JsonSerializerOptions(JsonSerializerDefaults.Web))),
                                                                    new SchemaRepository(null),
                                                                    typeof(Swashbuckle).GetMethods()[0]);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, null, null, null, null, key, 0, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);
            openApiRequestConfiguration.Execute(openApiOperation, operationFilterContext, new MetadataDefinition(metadataRouteDefinition,null,null, null));

            var openApiMediaType = openApiOperation.Responses.Where(x => x.Key == key.ToString())
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Content.Count.Should().Be(1);

            var openApiSchema = openApiMediaType.Content.Single();
            openApiSchema.Key.Should().Be(MediaTypeNames.Application.Octet);
            openApiSchema.Value.Schema.Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
        }
    }
}