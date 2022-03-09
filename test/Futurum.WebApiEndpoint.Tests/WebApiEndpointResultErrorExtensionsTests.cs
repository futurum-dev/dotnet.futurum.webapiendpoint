using System.Net;

using FluentAssertions;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class WebApiEndpointResultErrorExtensionsTests
{
    [Fact]
    public void without_DetailErrorMessage()
    {
        var httpStatusCode = HttpStatusCode.BadRequest;

        var resultError = httpStatusCode.ToResultError();

        resultError.Should().BeOfType<WebApiEndpointResultError>();

        var webApiEndpointResultError = resultError as WebApiEndpointResultError;
        webApiEndpointResultError.HttpStatusCode.Should().Be(httpStatusCode);
        webApiEndpointResultError.DetailErrorMessage.Should().BeEmpty();
    }
    
    [Fact]
    public void with_DetailErrorMessage()
    {
        var httpStatusCode = HttpStatusCode.BadRequest;
        var detailErrorMessage = Guid.NewGuid().ToString();

        var resultError = httpStatusCode.ToResultError(detailErrorMessage);

        resultError.Should().BeOfType<WebApiEndpointResultError>();

        var webApiEndpointResultError = resultError as WebApiEndpointResultError;
        webApiEndpointResultError.HttpStatusCode.Should().Be(httpStatusCode);
        webApiEndpointResultError.DetailErrorMessage.Should().Be(detailErrorMessage);
    }
}