using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestUploadFileWithPayloadOpenApiRequestConfigurationTests
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

            builder.Host.ConfigureServices((_, serviceCollection) => { serviceCollection.AddSingleton<RequestUploadFileWithPayloadOpenApiRequestConfiguration>(); });

            var application = builder.Build();

            var httpMethod = "GET";
            var route = string.Empty;

            var routeHandlerBuilder = application.MapMethods(route, new[] { httpMethod }, WebApiEndpointExecutor.ExecuteAsync);

            var endpointRouteBuilder = application as IEndpointRouteBuilder;

            var openApiRequestConfiguration = endpointRouteBuilder.ServiceProvider.GetService<RequestUploadFileWithPayloadOpenApiRequestConfiguration>();
            openApiRequestConfiguration.Execute(routeHandlerBuilder, metadataDefinition);

            var dataSource = endpointRouteBuilder.DataSources.OfType<EndpointDataSource>().FirstOrDefault();

            return dataSource.Endpoints.OfType<RouteEndpoint>().Single();
        }
    }

    public class Swashbuckle
    {
        public record CommandDto
        {
            [MapFromMultipartFile(0)] public IFormFile File { get; set; }

            [MapFromMultipartJson(1)] public PayloadDto Payload { get; set; }
        }
        public record PayloadDto(string Id);
        
        [Fact]
        public void check()
        {
            var openApiRequestConfiguration = new RequestUploadFileWithPayloadOpenApiRequestConfiguration();
            
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
            
            var metadataMapFromMultipartDefinition = new MetadataMapFromMultipartDefinition(WebApiEndpointMetadataTypeService.GetMapFromMultipartProperties(typeof(CommandDto))
                                                                                                                             .Select(x => new MetadataMapFromMultipartParameterDefinition(
                                                                                                                                 x.propertyInfo.Name, x.propertyInfo, x.mapFromMultipartAttribute))
                                                                                                                             .ToList());
            var metadataDefinition = new MetadataDefinition(null, null, null, metadataMapFromMultipartDefinition);

            openApiRequestConfiguration.Execute(openApiOperation, operationFilterContext, metadataDefinition);

            openApiOperation.RequestBody.Content.Count.Should().Be(1);
            var openApiMediaType = openApiOperation.RequestBody.Content.Where(x => x.Key == "multipart/form-data")
                                                   .Select(x => x.Value)
                                                   .Single();
            openApiMediaType.Schema.Type.Should().Be("object");
            openApiMediaType.Schema.Required.Should().BeEquivalentTo(nameof(CommandDto.File), nameof(CommandDto.Payload));
            openApiMediaType.Schema.Properties.Count.Should().Be(metadataMapFromMultipartDefinition.MapFromMultipartParameterDefinitions.Count);

            openApiMediaType.Schema.Properties[nameof(CommandDto.File)].Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
            openApiMediaType.Schema.Properties[nameof(CommandDto.Payload)].Should()
                            .BeEquivalentTo(new OpenApiSchema
                            {
                                Type = "object", Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    {
                                        nameof(PayloadDto.Id), new OpenApiSchema { Type = "string" }
                                    }
                                }
                            });
        }
    }
}