using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class MetadataRouteDefinitionBuilderFileUploadTests
{
    public class ApiEndpoint
    {
    }

    [Fact]
    public void when_specified()
    {
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                  0, false,
                                                                  Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
            .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
        resultMetadataRouteDefinition.AllowFileUploads.Should().BeTrue();
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
        resultMetadataRouteDefinition.AllowFileUploads.Should().BeFalse();
    }
}