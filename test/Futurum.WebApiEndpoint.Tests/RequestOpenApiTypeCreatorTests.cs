using FluentAssertions;

using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Xunit;
using Xunit.Abstractions;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestOpenApiTypeCreatorTests
{
    public record Complex(string Id, string FirstName, string LastName);

    public record Request(string Name, int Int, long Long, Complex Complex);

    private readonly ITestOutputHelper _output;

    public RequestOpenApiTypeCreatorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void when_no_MapFromParameterDefinitions_returns_type_with_same_properties()
    {
        var originalType = typeof(Request);

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());
        
        var newType = new RequestOpenApiTypeCreator().Create(metadataMapFromDefinition, originalType);

        var originalTypeProperties = originalType.GetProperties()
                                      .Select(propertyInfo => (propertyInfo.Name, propertyInfo.PropertyType));
        var newTypeProperties = newType.GetProperties()
                                 .Select(propertyInfo => (propertyInfo.Name, propertyInfo.PropertyType));

        var propertyDifferencesOriginalToNew = originalTypeProperties.Except(newTypeProperties);
        var propertyDifferencesNewToOriginal = newTypeProperties.Except(originalTypeProperties);

        propertyDifferencesOriginalToNew.Any().Should().BeFalse();
        propertyDifferencesNewToOriginal.Any().Should().BeFalse();
    }

    [Fact]
    public void when_MapFromParameterDefinitions_returns_type_with_same_properties_minus_specified()
    {
        var originalType = typeof(Request);

        var namePropertyName = nameof(Request.Name);
        var metadataMapFromParameterDefinition = new MetadataMapFromParameterDefinition(namePropertyName, originalType.GetProperties().Single(x => x.Name == namePropertyName), new MapFromPathAttribute(namePropertyName));
        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition> { metadataMapFromParameterDefinition });
        
        var newType = new RequestOpenApiTypeCreator().Create(metadataMapFromDefinition, originalType);

        var originalTypeProperties = originalType.GetProperties()
                                      .Select(propertyInfo => (propertyInfo.Name, propertyInfo.PropertyType));
        var newTypeProperties = newType.GetProperties()
                                 .Select(propertyInfo => (propertyInfo.Name, propertyInfo.PropertyType));

        var propertyDifferencesOriginalToNew = originalTypeProperties.Except(newTypeProperties).ToList();
        var propertyDifferencesNewToOriginal = newTypeProperties.Except(originalTypeProperties).ToList();

        propertyDifferencesOriginalToNew.Any().Should().BeTrue();
        propertyDifferencesOriginalToNew.Count.Should().Be(1);
        propertyDifferencesOriginalToNew[0].Name.Should().Be(namePropertyName);
        propertyDifferencesOriginalToNew[0].PropertyType.Should().Be(typeof(string));

        propertyDifferencesNewToOriginal.Any().Should().BeFalse();
    }

    [Fact]
    public void when_same_type_is_passed_to_it_it_uses_cached_type()
    {
        var originalType = typeof(Request);

        var metadataMapFromDefinition = new MetadataMapFromDefinition(new List<MetadataMapFromParameterDefinition>());

        var requestOpenApiTypeCreator = new RequestOpenApiTypeCreator();
        var newType1 = requestOpenApiTypeCreator.Create(metadataMapFromDefinition, originalType);
        var newType2 = requestOpenApiTypeCreator.Create(metadataMapFromDefinition, originalType);

        ReferenceEquals(newType1, newType2).Should().BeTrue();
    }
}