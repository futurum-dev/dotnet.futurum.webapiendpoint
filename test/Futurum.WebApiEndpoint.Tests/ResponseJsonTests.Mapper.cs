using System.Net.Mime;
using System.Text.Json;

using FluentAssertions;

using Futurum.Core.Option;
using Futurum.Test.Result;
using Futurum.WebApiEndpoint.Metadata;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

using Xunit;

namespace Futurum.WebApiEndpoint.Tests;

public class ResponseJsonMapperTests
{
    [Fact]
    public async Task Map()
    {
        var responseDataCollectionMapper = new ResponseJsonMapper<Data, DataDto, DataMapper>(Options.Create(new JsonOptions()), new DataMapper());

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Body = new MemoryStream();
        httpContext.Response.Body = new MemoryStream();

        var response = new Data("FirstName", 10);

        MetadataRouteDefinition metadataRouteDefinition =
            new(MetadataRouteHttpMethod.Get, string.Empty, null, new List<MetadataRouteParameterDefinition>(), null, 200, 400, Option<Action<RouteHandlerBuilder>>.None, null);

        var result = await responseDataCollectionMapper.MapAsync(httpContext, metadataRouteDefinition, response, CancellationToken.None);

        result.ShouldBeSuccess();

        httpContext.Response.StatusCode.Should().Be(metadataRouteDefinition.SuccessStatusCode);
        httpContext.Response.ContentType.Should().Be(MediaTypeNames.Application.Json);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        using var streamReader = new StreamReader(httpContext.Response.Body);
        var requestBody = await streamReader.ReadToEndAsync();

        var receivedResponse = JsonSerializer.Deserialize<DataDto>(requestBody, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        receivedResponse.Should().BeEquivalentTo(new DataDto("FirstName", 10));
    }

    public record Data(string FirstName, int Age);

    public record DataDto(string FirstName, int Age);

    private class DataMapper : IWebApiEndpointResponseDtoMapper<Data, DataDto>
    {
        public DataDto Map(Data data) =>
            new(data.FirstName, data.Age);
    }
}