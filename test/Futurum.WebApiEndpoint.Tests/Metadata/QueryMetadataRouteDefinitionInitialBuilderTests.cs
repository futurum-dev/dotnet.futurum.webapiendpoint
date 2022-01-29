using FluentAssertions;

using Futurum.WebApiEndpoint.Metadata;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class QueryMetadataRouteDefinitionInitialBuilderTests
{
    public class ApiEndpoint
    {
    }

    [Fact]
    public void check()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new QueryMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Route(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Get);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(503);
        metadataRouteDefinition.ParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void check_with_parameters()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);

        IMetadataRouteDefinitionBuilder queryMetadataRouteDefinitionsBuilder = new QueryMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                                               .Route(route, (parameterName, parameterType));

        var metadataRouteDefinitions = queryMetadataRouteDefinitionsBuilder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Get);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ParameterDefinitions[0].Type.Should().Be(parameterType);
    }
}