using System.Net;

using FluentAssertions;

using Futurum.Core.Result;
using Futurum.Test.Option;

using Microsoft.AspNetCore.WebUtilities;

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
        webApiEndpointResultError.Parent.ShouldBeHasValueWithValue(x => x.GetErrorString(),ReasonPhrases.GetReasonPhrase((int)httpStatusCode));
        webApiEndpointResultError.Children.Single().Should().BeOfType<ResultErrorEmpty>();
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
        webApiEndpointResultError.Parent.ShouldBeHasValueWithValue(x => x.GetErrorString(),ReasonPhrases.GetReasonPhrase((int)httpStatusCode));
        webApiEndpointResultError.Children.Single().ToErrorString().Should().Be(detailErrorMessage);
    }
}