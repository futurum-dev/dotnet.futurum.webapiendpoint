using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Option;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class MetadataRouteDefinitionBuilderExtendedOptionsTests
{
    public class ApiEndpoint
    {
    }

    [Fact]
    public void when_specified()
    {
        var action = (RouteHandlerBuilder builder) => { };
            
        var metadataRouteDefinition = new MetadataRouteDefinition(MetadataRouteHttpMethod.Get, string.Empty, null, new(), null, 0,
                                                                  0, Option<Action<RouteHandlerBuilder>>.None, Option<MetadataSecurityDefinition>.None);

        IMetadataRouteDefinitionBuilder builder = new MetadataRouteDefinitionBuilder(typeof(ApiEndpoint), metadataRouteDefinition)
            .ExtendedOptions(action);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var resultMetadataRouteDefinition = metadataRouteDefinitions.First();
        resultMetadataRouteDefinition.ExtendedOptions.Should().Be(action);
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
        resultMetadataRouteDefinition.ExtendedOptions.ShouldBeHasNoValue();
    }
}