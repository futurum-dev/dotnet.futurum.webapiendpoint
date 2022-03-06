using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Internal;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests.Internal;

public class WebApiEndpointHttpContextDispatcherTests
{
    [Fact]
    public async Task success()
    {
        const string ErrorMessage = "ERROR-MESSAGE";

        var resultError = ErrorMessage.ToResultError();

        var webApiEndpointHttpContextDispatcher = new WebApiEndpointHttpContextDispatcher(Options.Create(new JsonOptions()));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream();
        httpContext.Response.Body = new MemoryStream();

        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var result = await webApiEndpointHttpContextDispatcher.HandleFailedResponseAsync(httpContext, resultError, metadataRouteDefinition, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.FailedStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var resultErrorStructure = JsonSerializer.Deserialize<ResultErrorStructure>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        resultErrorStructure.Message.Should().Be(ErrorMessage);
        resultErrorStructure.Children.Should().BeEmpty();
    }
}