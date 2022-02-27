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
        public void when_specified_first()
        {
            var summary = Guid.NewGuid().ToString();
            
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Summary(summary);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Summary.Should().Be(summary);
        }
        
        [Fact]
        public void when_specified_not_first()
        {
            var summary = Guid.NewGuid().ToString();
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, string.Empty, false, null);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0,
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
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
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
        public void when_specified_first()
        {
            var description = Guid.NewGuid().ToString();
            
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Description(description);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Description.Should().Be(description);
        }

        [Fact]
        public void when_specified_not_first()
        {
            var description = Guid.NewGuid().ToString();
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, string.Empty, false, null);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0,
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
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }

    public class Deprecated
    {
        [Fact]
        public void when_specified_first()
        {
            var deprecated = true;
            
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Deprecated(deprecated);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Deprecated.Should().Be(deprecated);
        }

        [Fact]
        public void when_specified_not_first()
        {
            var deprecated = true;
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, string.Empty, false, null);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .Deprecated(deprecated);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Deprecated.Should().Be(deprecated);
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }

    public class ExternalDocsDescription
    {
        [Fact]
        public void when_specified_first()
        {
            var externalDocsDescription = Guid.NewGuid().ToString();
            
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .ExternalDocsDescription(externalDocsDescription);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.ExternalDocs.Description.Should().Be(externalDocsDescription);
        }

        [Fact]
        public void when_specified_not_first()
        {
            var externalDocsDescription = Guid.NewGuid().ToString();
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, string.Empty, false, null);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .ExternalDocsDescription(externalDocsDescription);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.ExternalDocs.Description.Should().Be(externalDocsDescription);
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }

    public class ExternalDocsUrl
    {
        [Fact]
        public void when_specified_first()
        {
            var externalDocsUrl = new Uri("http://www.google.com");
            
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .ExternalDocsUrl(externalDocsUrl);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.ExternalDocs.Url.Should().Be(externalDocsUrl);
        }

        [Fact]
        public void when_specified_not_first()
        {
            var externalDocsUrl = new Uri("https://www.google.com");
            
            var metadataRouteOpenApiOperation = new MetadataRouteOpenApiOperation(string.Empty, string.Empty, false, null);
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), metadataRouteOpenApiOperation, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .ExternalDocsUrl(externalDocsUrl);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.ExternalDocs.Url.Should().Be(externalDocsUrl);
        }

        [Fact]
        public void when_not_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.OpenApiOperation.Should().BeNull();
        }
    }
}