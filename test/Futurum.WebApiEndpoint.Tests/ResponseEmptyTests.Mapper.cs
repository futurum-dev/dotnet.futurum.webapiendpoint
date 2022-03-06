using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseEmptyMapperTests
{
    [Fact]
    public async Task success()
    {
        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var responseEmptyMapper = new ResponseEmptyMapper();

        var httpContext = new DefaultHttpContext();

        var result = await responseEmptyMapper.MapAsync(httpContext, metadataRouteDefinition, ResponseEmpty.Default, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
        httpContext.Response.ContentType.Should().BeNull();
    }
}