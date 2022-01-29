using System.Net;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class MetadataRouteDefinitionBuilderStatusCodeTests
{
    public class ApiEndpoint
    {
    }

    public class SuccessStatusCode
    {
        [Fact]
        public void when_number_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var statusCode = 100;

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .SuccessStatusCode(statusCode);
            
            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SuccessStatusCode.Should().Be(statusCode);
        }

        [Fact]
        public void when_enum_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .SuccessStatusCode(HttpStatusCode.Forbidden);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.SuccessStatusCode.Should().Be((int)HttpStatusCode.Forbidden);
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
            resultMetadataRouteDefinition.SuccessStatusCode.Should().Be(0);
        }
    }

    public class FailedStatusCode
    {
        [Fact]
        public void when_number_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            var statusCode = 100;

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .FailedStatusCode(statusCode);
            
            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.FailedStatusCode.Should().Be(statusCode);
        }

        [Fact]
        public void when_enum_specified()
        {
            var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0, 0, false,
                                                                      Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

            IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
                .FailedStatusCode(HttpStatusCode.Forbidden);

            var metadataRouteDefinitions = builder.Build();

            metadataRouteDefinitions.Count().Should().Be(1);

            var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
            resultMetadataRouteDefinition.FailedStatusCode.Should().Be((int)HttpStatusCode.Forbidden);
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
            resultMetadataRouteDefinition.FailedStatusCode.Should().Be(0);
        }
    }
}