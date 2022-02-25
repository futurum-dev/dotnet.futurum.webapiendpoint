using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseFileStreamMapperTests
{
    [Fact]
    public void Map()
    {
        var fileInfo = new FileInfo(Guid.NewGuid().ToString());
        var contentType = Guid.NewGuid().ToString();

        var result = new ResponseFileStreamMapper<object>().Map(new ResponseFileStream<object>(fileInfo, contentType));

        result.Should().BeEquivalentTo(new ResponseFileStreamDto(fileInfo, contentType));
    }
}