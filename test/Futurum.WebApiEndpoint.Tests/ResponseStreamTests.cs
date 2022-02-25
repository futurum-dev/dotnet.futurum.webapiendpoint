using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseStreamTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var stream = new MemoryStream();
        var fileName = Guid.NewGuid().ToString();
        var fileLengthBytes = 10;
        var contentType = Guid.NewGuid().ToString();

        var nonGeneric = new ResponseStream(stream, fileName, fileLengthBytes, contentType);

        var generic = nonGeneric.ToApiEndpoint<object>();

        generic.Stream.Should().BeSameAs(nonGeneric.Stream);
        generic.FileName.Should().Be(nonGeneric.FileName);
        generic.FileLengthBytes.Should().Be(nonGeneric.FileLengthBytes);
        generic.ContentType.Should().Be(nonGeneric.ContentType);
    }
}