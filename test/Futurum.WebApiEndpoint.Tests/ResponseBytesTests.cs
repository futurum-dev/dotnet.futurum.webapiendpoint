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

        generic.Bytes.Should().BeSameAs(nonGeneric.Bytes);
        generic.FileName.Should().Be(nonGeneric.FileName);
        generic.ContentType.Should().Be(nonGeneric.ContentType);
    }
}