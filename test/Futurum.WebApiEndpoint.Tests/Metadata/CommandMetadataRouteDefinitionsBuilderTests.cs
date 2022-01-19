using FluentAssertions;

using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Mvc;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Metadata;

public class CommandMetadataRouteDefinitionsBuilderTests
{
    private class ApiEndpoint
    {
    }

    [Fact]
    public void post()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Post(route)
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(400);
        version1MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(400);
        version2MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void post_with_parameters()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Post(route, (parameterName, parameterType))
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(400);
        version1MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Post);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(400);
        version2MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void put()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Put(route)
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void put_with_parameters()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Put(route, (parameterName, parameterType))
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Put);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(201);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void patch()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Patch(route)
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void patch_with_parameters()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Patch(route, (parameterName, parameterType))
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Patch);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void delete()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Delete(route)
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Should().BeEmpty();
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }

    [Fact]
    public void delete_with_parameters()
    {
        var route = Guid.NewGuid().ToString();
        var apiVersion1 = new ApiVersion(1, 0);
        var apiVersion2 = new ApiVersion(2, 0);
        var summary = Guid.NewGuid().ToString();
        var description = Guid.NewGuid().ToString();
        var parameterName = "parameter1";
        var parameterType = typeof(int);

        IMetadataRouteDefinitionBuilder builder = new CommandMetadataRouteDefinitionBuilder(typeof(ApiEndpoint))
                                                  .Delete(route, (parameterName, parameterType))
                                                  .Version(apiVersion1, apiVersion2)
                                                  .Summary(summary)
                                                  .Description(description)
                                                  .AllowFileUploads();

        var metadataRouteDefinitions = builder.Build();

        metadataRouteDefinitions.Count().Should().Be(2);

        var version1MetadataRouteDefinitions = metadataRouteDefinitions.First();
        version1MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version1MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion1);
        version1MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version1MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version1MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        version1MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version1MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version1MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version1MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version1MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();

        var version2MetadataRouteDefinitions = metadataRouteDefinitions.Skip(1).First();
        version2MetadataRouteDefinitions.RouteTemplate.Should().Be(route);
        version2MetadataRouteDefinitions.ApiVersion.Should().Be(apiVersion2);
        version2MetadataRouteDefinitions.OpenApiOperation.Summary.Should().Be(summary);
        version2MetadataRouteDefinitions.OpenApiOperation.Description.Should().Be(description);
        version2MetadataRouteDefinitions.HttpMethod.Should().Be(MetadataRouteHttpMethod.Delete);
        version2MetadataRouteDefinitions.SuccessStatusCode.Should().Be(200);
        version2MetadataRouteDefinitions.FailedStatusCode.Should().Be(404);
        version2MetadataRouteDefinitions.ParameterDefinitions.Count.Should().Be(1);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Name.Should().Be(parameterName);
        version2MetadataRouteDefinitions.ParameterDefinitions[0].Type.Should().Be(parameterType);
        version2MetadataRouteDefinitions.AllowFileUploads.Should().BeTrue();
    }
}