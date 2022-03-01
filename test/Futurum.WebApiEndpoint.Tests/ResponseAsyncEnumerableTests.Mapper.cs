using System.Text.Json;

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

public class ResponseAsyncEnumerableMapperTests
{
    [Fact]
    public async Task Map()
    {
        var responseAsyncEnumerableMapper = new ResponseAsyncEnumerableMapper<object, Data, DataDto, DataMapper>(Options.Create(new JsonOptions()), new DataMapper());

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream();
        httpContext.Response.Body = new MemoryStream();

        var data = Enumerable.Range(0, 10)
                             .Select(i => new Data($"First{i}", i))
                             .ToAsyncEnumerable();

        var response = new ResponseAsyncEnumerable<object, Data>(data);

        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var result = await responseAsyncEnumerableMapper.MapAsync(httpContext, metadataRouteDefinition, response, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var receivedResponse = JsonSerializer.Deserialize<IEnumerable<DataDto>>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        receivedResponse.Should().BeEquivalentTo(Enumerable.Range(0, 10)
                                                           .Select(i => new DataDto($"First{i}", i))
                                                           .ToList());
    }

    public record Data(string FirstName, int Age);

    public record DataDto(string FirstName, int Age);

    private class DataMapper : IWebApiEndpointResponseDataMapper<Data, DataDto>
    {
        public DataDto Map(Data data) =>
            new(data.FirstName, data.Age);
    }
}