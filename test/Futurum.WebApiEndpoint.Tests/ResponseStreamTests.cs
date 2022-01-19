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

        nonGeneric.Stream.Should().BeSameAs(generic.Stream);
        nonGeneric.FileName.Should().Be(generic.FileName);
        nonGeneric.FileLengthBytes.Should().Be(generic.FileLengthBytes);
        nonGeneric.ContentType.Should().Be(generic.ContentType);
    }
}