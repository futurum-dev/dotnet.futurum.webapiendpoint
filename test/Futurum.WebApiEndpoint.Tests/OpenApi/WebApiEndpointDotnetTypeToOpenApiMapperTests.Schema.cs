using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointDotnetTypeToOpenApiSchemaMapperTests
{
    public static class Type
    {
        public const string String = "string";
        public const string Integer = "integer";
        public const string Number = "number";
    }

    [Theory]
    [InlineData(typeof(string), Type.String, null)]
    [InlineData(typeof(int), Type.Integer, "int32")]
    [InlineData(typeof(long), Type.Integer, "int64")]
    [InlineData(typeof(float), Type.Number, "float")]
    [InlineData(typeof(double), Type.Number, "double")]
    [InlineData(typeof(byte), Type.String, "byte")]
    [InlineData(typeof(DateTime), Type.String, "date-time")]
    [InlineData(typeof(bool), "boolean", null)]
    [InlineData(typeof(Guid), Type.String, "uuid")]
    [InlineData(typeof(IFormFile), Type.String, "binary")]
    [InlineData(typeof(WebApiEndpoint.Range), Type.String, null)]
    [InlineData(typeof(Option<WebApiEndpoint.Range>), Type.String, null)]
    public void check(System.Type type, string schemaType, string? schemaFormat)
    {
        var openApiSchema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(type);

        openApiSchema.Type.Should().Be(schemaType);
        openApiSchema.Format.Should().Be(schemaFormat);
    }

    [Fact]
    public void check_IEnumerable_IFormFile()
    {
        var openApiSchema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(IEnumerable<IFormFile>));

        openApiSchema.Type.Should().Be("array");
        openApiSchema.Items.Should().BeEquivalentTo(new OpenApiSchema { Type = "string", Format = "binary" });
    }

    [Fact]
    public void check_custom_class()
    {
        var openApiSchema = WebApiEndpointDotnetTypeToOpenApiSchemaMapper.Execute(typeof(CustomClass));

        openApiSchema.Type.Should().Be("object");
        openApiSchema.Properties.Should().BeEquivalentTo(new Dictionary<string, OpenApiSchema>
        {
            { nameof(CustomClass.String), new OpenApiSchema { Type = Type.String } },
            { nameof(CustomClass.Int), new OpenApiSchema { Type = Type.Integer, Format = "int32" } },
            { nameof(CustomClass.Long), new OpenApiSchema { Type = Type.Integer, Format = "int64" } },
            { nameof(CustomClass.DateTime), new OpenApiSchema { Type = Type.String, Format = "date-time" } },
            { nameof(CustomClass.Boolean), new OpenApiSchema { Type = "boolean" } },
        });
    }

    public class CustomClass
    {
        public string String { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public DateTime DateTime { get; set; }
        public bool Boolean { get; set; }
    }
}