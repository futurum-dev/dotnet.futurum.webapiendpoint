using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseEmptyJsonTests
{
    [Fact]
    public void ToApiEndpoint()
    {
        var nonGeneric = new ResponseEmptyJson();

        var generic = nonGeneric.ToApiEndpoint<object>();

        generic.Should().NotBeNull();
    }
}