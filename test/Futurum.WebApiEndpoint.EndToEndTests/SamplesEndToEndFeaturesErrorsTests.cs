using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Sample.Features.Error;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Futurum.WebApiEndpoint.EndToEndTests;

[Collection("Sequential")]
public class SamplesEndToEndFeaturesErrorsTests
{
    [Fact]
    public async Task ErrorResult()
    {
        var httpClient = CreateClient();

        var commandDto = new ErrorResultScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/error-result"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        response.Title.Should().Be($"An result error has occured - {commandDto.Id}");
    }
    
    [Fact]
    public async Task ErrorException()
    {
        var httpClient = CreateClient();

        var commandDto = new ErrorExceptionScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/error-exception"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ProblemDetails>();

        response.Title.Should().Be("WebApiEndpoint - Internal Server Error");
        response.Detail.Should().Contain($"An exception has occured - {commandDto.Id}");
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();
}