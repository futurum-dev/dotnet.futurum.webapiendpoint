using FluentAssertions;

using Futurum.Core.Option;
using Futurum.WebApiEndpoint.OpenApi;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.OpenApi;

public class WebApiEndpointDotnetTypeToOpenApiIsRequiredMapperTests
{
    [Theory]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(long), true)]
    [InlineData(typeof(float), true)]
    [InlineData(typeof(double), true)]
    [InlineData(typeof(byte), true)]
    [InlineData(typeof(DateTime), true)]
    [InlineData(typeof(bool), true)]
    [InlineData(typeof(Guid), true)]
    [InlineData(typeof(IFormFile), true)]
    [InlineData(typeof(WebApiEndpoint.Range), true)]
    [InlineData(typeof(Option<WebApiEndpoint.Range>), false)]
    [InlineData(typeof(IEnumerable<IFormFile>), true)]
    public void check(Type type, bool expectedIsRequired)
    {
        var isRequired = WebApiEndpointDotnetTypeToOpenApiIsRequiredMapper.Execute(type);

        isRequired.Should().Be(expectedIsRequired);
    }
}