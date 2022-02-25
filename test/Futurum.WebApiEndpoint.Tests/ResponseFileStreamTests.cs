using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseFileStreamTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var fileInfo = new FileInfo(Guid.NewGuid().ToString());
        var contentType = Guid.NewGuid().ToString();

        var nonGeneric = new ResponseFileStream(fileInfo, contentType);

        var generic = nonGeneric.ToApiEndpoint<object>();

        generic.FileInfo.Should().Be(nonGeneric.FileInfo);
        generic.ContentType.Should().Be(nonGeneric.ContentType);
    }
}