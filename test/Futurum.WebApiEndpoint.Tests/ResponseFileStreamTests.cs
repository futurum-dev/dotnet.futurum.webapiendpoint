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

        nonGeneric.FileInfo.Should().Be(generic.FileInfo);
        nonGeneric.ContentType.Should().Be(generic.ContentType);
    }
}