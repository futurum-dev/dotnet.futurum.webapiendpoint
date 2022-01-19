using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class RequestPlainTextTests
{
    [Fact]
    public void ToNonApiEndpoint()
    {
        var input = Guid.NewGuid().ToString();

        var generic = new RequestPlainText<object>(input);

        var nonGeneric = generic.ToNonApiEndpoint();

        generic.Body.Should().Be(nonGeneric.Body);
    }
}