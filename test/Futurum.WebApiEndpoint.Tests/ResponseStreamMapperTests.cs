using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseStreamMapperTests
{
    [Fact]
    public void Map()
    {
        var stream = new MemoryStream();
        var fileName = Guid.NewGuid().ToString();
        var fileLengthBytes = 10;
        var contentType = Guid.NewGuid().ToString();

        var result = new ResponseStreamMapper<object>().Map(new ResponseStream<object>(stream, fileName, fileLengthBytes, contentType));

        result.Should().BeEquivalentTo(new ResponseStreamDto(stream, contentType, fileName, fileLengthBytes));
    }
}