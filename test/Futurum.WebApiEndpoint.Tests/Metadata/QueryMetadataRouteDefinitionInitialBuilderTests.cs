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
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void check_with_parameters()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new QueryMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .RouteWithParameters(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Get);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void check_with_parameters_specified()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);
        var parameterDefinitionType = MetadataRouteParameterDefinitionType.Path;

        IMetadataRouteDefinitionBuilder queryMetadataRouteDefinitionsBuilder = new QueryMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                                               .Route(route, (parameterName, parameterDefinitionType, parameterType));

        var metadataRouteDefinitions = queryMetadataRouteDefinitionsBuilder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Get);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ManualParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ManualParameterDefinitions[0].ParameterDefinitionType.Should().Be(parameterDefinitionType);
        metadataRouteDefinition.ManualParameterDefinitions[0].Type.Should().Be(parameterType);
    }
}