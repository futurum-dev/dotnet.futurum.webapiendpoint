using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseEmptyJsonMapperTests
{
    [Fact]
    public void Map()
    {
        var result = new ResponseEmptyJsonMapper<object>().Map(new ResponseEmptyJson<object>());

        result.Should().NotBeNull();
    }
}