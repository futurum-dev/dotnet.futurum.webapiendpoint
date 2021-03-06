using FluentAssertions;

using Futurum.WebApiEndpoint.Metadata;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class CommandMetadataRouteDefinitionInitialBuilderTests
{
    private class ApiEndpoint
    {
    }

    [Fact]
    public void post()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Post(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(201);
        metadataRouteDefinition.FailedStatusCode.Should().Be(400);
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();;
    }

    [Fact]
    public void post_with_parameters_specified()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);
        var parameterDefinitionType = MetadataRouteParameterDefinitionType.Path;
        
        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Post(route, (parameterName, parameterDefinitionType, parameterType));

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(201);
        metadataRouteDefinition.FailedStatusCode.Should().Be(400);
        metadataRouteDefinition.ManualParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ManualParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ManualParameterDefinitions[0].ParameterDefinitionType.Should().Be(parameterDefinitionType);
        metadataRouteDefinition.ManualParameterDefinitions[0].Type.Should().Be(parameterType);
    }

    [Fact]
    public void put()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Put(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(201);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void put_with_parameters_specified()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);
        var parameterDefinitionType = MetadataRouteParameterDefinitionType.Path;

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Put(route, (parameterName, parameterDefinitionType, parameterType));

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(201);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ManualParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ManualParameterDefinitions[0].ParameterDefinitionType.Should().Be(parameterDefinitionType);
        metadataRouteDefinition.ManualParameterDefinitions[0].Type.Should().Be(parameterType);
    }

    [Fact]
    public void patch()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Patch(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void patch_with_parameters_specified()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);
        var parameterDefinitionType = MetadataRouteParameterDefinitionType.Path;

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Patch(route, (parameterName, parameterDefinitionType, parameterType));

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ManualParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ManualParameterDefinitions[0].ParameterDefinitionType.Should().Be(parameterDefinitionType);
        metadataRouteDefinition.ManualParameterDefinitions[0].Type.Should().Be(parameterType);
    }

    [Fact]
    public void delete()
    {
        var route = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Delete(route);

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Should().BeEmpty();
    }

    [Fact]
    public void delete_with_parameters_specified()
    {
        var route = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);
        var parameterDefinitionType = MetadataRouteParameterDefinitionType.Path;

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionInitialBuilder(typeof(ApiEndpoint))
                                                  .Delete(route, (parameterName, parameterDefinitionType, parameterType));

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(1);

        var metadataRouteDefinition = metadataRouteDefinitions.First();
        metadataRouteDefinition.RouteTemplate.Should().Be(route);
        metadataRouteDefinition.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        metadataRouteDefinition.SuccessStatusCode.Should().Be(200);
        metadataRouteDefinition.FailedStatusCode.Should().Be(404);
        metadataRouteDefinition.ManualParameterDefinitions.Count.Should().Be(1);
        metadataRouteDefinition.ManualParameterDefinitions[0].Name.Should().Be(parameterName);
        metadataRouteDefinition.ManualParameterDefinitions[0].ParameterDefinitionType.Should().Be(parameterDefinitionType);
        metadataRouteDefinition.ManualParameterDefinitions[0].Type.Should().Be(parameterType);
    }
}