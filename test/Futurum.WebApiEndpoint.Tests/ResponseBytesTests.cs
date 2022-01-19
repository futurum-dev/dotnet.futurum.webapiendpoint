using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseBytesTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var bytes = new byte[]{};
        var fileName = Guid.NewGuid().ToString();
        var contentType = Guid.NewGuid().ToString();

        var nonGeneric = new ResponseBytes(bytes, fileName, contentType);

        var generic = nonGeneric.ToApiEndpoint<object>();

        nonGeneric.Bytes.Should().BeSameAs(generic.Bytes);
        nonGeneric.FileName.Should().Be(generic.FileName);
        nonGeneric.ContentType.Should().Be(generic.ContentType);
    }
}