using System.Text.Json;
using System.Text.Json.Nodes;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseEmptyJsonMapperTests
{
    [Fact]
    public async Task Map()
    {
        var responseEmptyJsonMapper = new ResponseEmptyJsonMapper<object>(Options.Create(new JsonOptions()));

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream();
        httpContext.Response.Body = new MemoryStream();

        var response = new ResponseEmptyJson<object>();

        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var result = await responseEmptyJsonMapper.MapAsync(httpContext, metadataRouteDefinition, response, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var receivedResponse = JsonSerializer.Deserialize<JsonObject>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        receivedResponse.Should().NotBeNull();
        receivedResponse.ToJsonString().Should().Be("{}");
    }
}