using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

using FluentAssertions;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Sample.Features;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;
using Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Futurum.WebApiEndpoint.EndToEndTests;

[Collection("Sequential")]
public class SamplesEndToEndFeaturesResponsesTests
{
    [Fact]
    public async Task Json()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-with-response"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

        response.Name.Should().Be($"Name - {commandDto.Id}");
    }

    [Fact]
    public async Task AsyncEnumerable()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-async-enumerable/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<FeatureDto>>();

        response.Should().BeEquivalentTo(Enumerable.Range(0, 10).Select(i => new FeatureDto($"Name - {i} - 2")));
    }

    [Fact]
    public async Task Bytes()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-bytes/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

        await using var expectedFileStream = File.OpenRead("./Data/hello-world.txt");

        var expectedBytes = new byte[expectedFileStream.Length];
        await expectedFileStream.ReadAsync(expectedBytes, CancellationToken.None);

        bytes.Should().BeEquivalentTo(expectedBytes);
        httpResponseMessage.Content.Headers.GetValues("Content-Disposition").Single().Should().Be("attachment; filename=hello-world-bytes-2");
        httpResponseMessage.Content.Headers.GetValues("Content-Type").Single().Should().Be(MediaTypeNames.Application.Octet);
        httpResponseMessage.Content.Headers.GetValues("Content-Length").Single().Should().Be(expectedFileStream.Length.ToString());
    }

    public class Range
    {
        public class Manual
        {
            [Collection("Sequential")]
            public class Range_type
            {
                [Fact]
                public async Task not_set()
                {
                    const string expected = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-manual-parameter-with-response-bytes-range/{expected}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<ResultErrorStructure>();

                    response.Message.Should().Be("Unable to get range");
                }

                [Theory]
                [InlineData(0, 5, "abcdef")]
                [InlineData(1, 5, "bcdef")]
                [InlineData(20, 25, "uvwxyz")]
                public async Task set(int from, int to, string expectedResult)
                {
                    const string expectedComplete = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var range = $"bytes={from}-{to}";

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Range", range);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-manual-parameter-with-response-bytes-range/{expectedComplete}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expectedResult);
                }
            }

            [Collection("Sequential")]
            public class Option_Range_type
            {
                [Fact]
                public async Task not_set()
                {
                    const string expected = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-manual-parameter-with-response-bytes-option-range/{expected}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expected);
                }

                [Theory]
                [InlineData(0, 5, "abcdef")]
                [InlineData(1, 5, "bcdef")]
                [InlineData(20, 25, "uvwxyz")]
                public async Task set(int from, int to, string expectedResult)
                {
                    const string expectedComplete = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var range = $"bytes={from}-{to}";

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Range", range);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-manual-parameter-with-response-bytes-option-range/{expectedComplete}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expectedResult);
                }
            }
        }

        public class MapFrom
        {
            [Collection("Sequential")]
            public class Range_type
            {
                [Fact]
                public async Task not_set()
                {
                    const string expected = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-parameter-map-from-with-response-bytes-range/{expected}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
                    var response = await httpResponseMessage.Content.ReadFromJsonAsync<ResultErrorStructure>();

                    response.Message.Should()
                            .Be(
                                $"Failed to MapFromHeader for {typeof(WebApiEndpoint.Range).FullName} for property : '{nameof(QueryWithRequestParameterMapFromWithResponseBytesRangeScenario.RequestDto.Range)}'");
                }

                [Theory]
                [InlineData(0, 5, "abcdef")]
                [InlineData(1, 5, "bcdef")]
                [InlineData(20, 25, "uvwxyz")]
                public async Task set(int from, int to, string expectedResult)
                {
                    const string expectedComplete = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var range = $"bytes={from}-{to}";

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Range", range);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-parameter-map-from-with-response-bytes-range/{expectedComplete}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expectedResult);
                }
            }

            [Collection("Sequential")]
            public class Option_Range_type
            {
                [Fact]
                public async Task not_set()
                {
                    const string expected = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-parameter-map-from-with-response-bytes-option-range/{expected}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expected);
                }

                [Theory]
                [InlineData(0, 5, "abcdef")]
                [InlineData(1, 5, "bcdef")]
                [InlineData(20, 25, "uvwxyz")]
                public async Task set(int from, int to, string expectedResult)
                {
                    const string expectedComplete = "abcdefghijklmnopqrstuvwxyz";

                    var httpClient = CreateClient();

                    var range = $"bytes={from}-{to}";

                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Range", range);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri($"/api/1.0/query-with-request-parameter-map-from-with-response-bytes-option-range/{expectedComplete}")
                    };

                    var httpResponseMessage = await httpClient.SendAsync(request);

                    httpResponseMessage.EnsureSuccessStatusCode();
                    var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                    var result = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    result.Should().Be(expectedResult);
                }
            }
        }
    }

    [Fact]
    public async Task DataCollection()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-data-collection/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<ResponseDataCollectionDto<FeatureDto>>();

        response.Count.Should().Be(10);
        response.Data.Should().BeEquivalentTo(Enumerable.Range(0, 10).Select(i => new FeatureDto($"Name - {i} - 2")));
    }

    [Fact]
    public async Task EmptyJson()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-empty-json/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var response = await httpResponseMessage.Content.ReadFromJsonAsync<JsonObject>();

        response.Should().NotBeNull();
        response.ToJsonString().Should().Be("{}");
    }

    [Fact]
    public async Task FileStream()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-file-stream/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

        await using var expectedFileStream = File.OpenRead("./Data/hello-world.txt");

        var expectedBytes = new byte[expectedFileStream.Length];
        await expectedFileStream.ReadAsync(expectedBytes, CancellationToken.None);

        bytes.Should().BeEquivalentTo(expectedBytes);
        httpResponseMessage.Content.Headers.GetValues("Content-Disposition").Single().Should().Be("attachment; filename=hello-world.txt");
        httpResponseMessage.Content.Headers.GetValues("Content-Type").Single().Should().Be(MediaTypeNames.Application.Octet);
        httpResponseMessage.Content.Headers.GetValues("Content-Length").Single().Should().Be(expectedFileStream.Length.ToString());
    }

    [Fact]
    public async Task Stream()
    {
        var httpClient = CreateClient();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-manual-parameter-with-response-stream/2"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();

        await using var expectedFileStream = File.OpenRead("./Data/hello-world.txt");

        var expectedBytes = new byte[expectedFileStream.Length];
        await expectedFileStream.ReadAsync(expectedBytes, CancellationToken.None);

        bytes.Should().BeEquivalentTo(expectedBytes);
        httpResponseMessage.Content.Headers.GetValues("Content-Disposition").Single().Should().Be("attachment; filename=hello-world-stream-2");
        httpResponseMessage.Content.Headers.GetValues("Content-Type").Single().Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Redirect()
    {
        var httpClient = CreateClientWithoutRedirect();

        var commandDto = new CommandWithRequestWithResponseScenario.CommandDto(Guid.NewGuid().ToString());
        var json = JsonSerializer.Serialize(commandDto);

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/command-with-request-with-response-redirect"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await httpClient.SendAsync(request);

        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Moved);
        httpResponseMessage.Headers.Location.OriginalString.Should().Be("api/1.0/command-with-request-with-response");
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient();

    private static HttpClient CreateClientWithoutRedirect() =>
        new WebApplicationFactory<Sample.Program>()
            .CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
}