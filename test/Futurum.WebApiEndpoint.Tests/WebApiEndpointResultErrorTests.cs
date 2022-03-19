using System.Net;

using FluentAssertions;

using Microsoft.AspNetCore.WebUtilities;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class WebApiEndpointResultErrorTests
{
    [Fact]
    public void GetErrorString()
    {
        var httpStatusCode = HttpStatusCode.BadRequest;
        var detailErrorMessage = Guid.NewGuid().ToString();

        var webApiEndpointResultError = new WebApiEndpointResultError(httpStatusCode, detailErrorMessage);

        var errorString = webApiEndpointResultError.GetErrorString(";");

        errorString.Should().Be($"{ReasonPhrases.GetReasonPhrase((int)httpStatusCode)};{detailErrorMessage}");
    }
    
    [Fact]
    public void GetErrorStructure()
    {
        var httpStatusCode = HttpStatusCode.BadRequest;
        var detailErrorMessage = Guid.NewGuid().ToString();

        var webApiEndpointResultError = new WebApiEndpointResultError(httpStatusCode, detailErrorMessage);

        var errorStructure = webApiEndpointResultError.GetErrorStructure();

        errorStructure.Message.Should().Be(ReasonPhrases.GetReasonPhrase((int)httpStatusCode));
        errorStructure.Children.First().Message.Should().Be(detailErrorMessage);
        errorStructure.Children.First().Children.Should().BeEmpty();
    }
}