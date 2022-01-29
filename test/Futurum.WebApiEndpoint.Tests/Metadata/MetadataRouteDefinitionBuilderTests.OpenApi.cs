using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class MetadataRouteDefinitionBuilderOpenApiTests
{
    public class ApiEndpoint
    {
    }

    public class Summary
    {
        [Fact]
        public void when_specified()
        {
            var summary = Guid.NewGuid().ToString();
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(summary, string.Empty);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Summary(summary);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Summary.Should().Be(summary);
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }

    public class Description
    {
        [Fact]
        public void when_specified()
        {
            var description = Guid.NewGuid().ToString();
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, description);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Description(description);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Description.Should().Be(description);
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }
}