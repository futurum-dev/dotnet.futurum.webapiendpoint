using FluentAssertions;

using Futurum.Test.Result;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseBytesMapperTests
{
    [Fact]
    public void Map()
    {
        var bytes = new byte[]{};
        var fileName = Guid.NewGuid().ToString();
        var contentType = Guid.NewGuid().ToString();

        var result = new ResponseBytesMapper<object>().Map(new ResponseBytes<object>(bytes, fileName, contentType));

        result.Should().BeEquivalentTo(new ResponseBytesDto(bytes, contentType, fileName));
    }
}