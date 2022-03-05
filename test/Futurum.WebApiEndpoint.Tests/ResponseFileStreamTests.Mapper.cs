using System.Net.Mime;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseFileStreamMapperTests
{
    [Fact]
    public async Task Map()
    {
        var responseFileStreamMapper = new ResponseFileStreamMapper();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream();
        httpContext.Response.Body = new MemoryStream();

        var fileStream = new FileInfo("./Data/hello-world.txt").OpenRead();

        var sentBytes = new byte[fileStream.Length];
        await fileStream.ReadAsync(sentBytes, CancellationToken.None);

        var response = new ResponseFileStream(new FileInfo("./Data/hello-world.txt"))
        {
            ContentType = MediaTypeNames.Application.Octet
        };

        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var result = await responseFileStreamMapper.MapAsync(httpContext, metadataRouteDefinition, response, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var receivedBytes = new byte[fileStream.Length];

        await httpContext.Response.Body.ReadAsync(receivedBytes, CancellationToken.None);

        receivedBytes.Should().BeEquivalentTo(sentBytes);
    }
}