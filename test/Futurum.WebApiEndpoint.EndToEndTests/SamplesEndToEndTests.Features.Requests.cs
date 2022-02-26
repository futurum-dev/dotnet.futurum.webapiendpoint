using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Sample.Features;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;
using Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;
using Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;

namespace Futurum.WebApiEndpoint.EndToEndTests;

public class SamplesEndToEndFeaturesRequestsTests
{
    [Collection("Sequential")]
    public class Query
    {
        [Fact]
        public async Task WithoutRequest()
        {
            var httpClient = CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/1.0/query-without-request-with-response"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be("Name - 0");
        }

        [Fact]
        public async Task RequestParameterMapFrom()
        {
            var httpClient = CreateClient();

            var input = 2;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"/api/1.0/query-with-request-parameter-map-from-with-response/{input}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be("Name - 0 - Request { Id = 2 }");
        }

        [Fact]
        public async Task RequestParameterMapFromSupportedTypes()
        {
            var httpClient = CreateClient();

            var stringValue = Guid.NewGuid().ToString();
            var intValue = int.MaxValue;
            var longValue = long.MaxValue;
            var dateTimeValue = DateTime.Now;
            var boolValue = true;
            var guidValue = Guid.NewGuid();

            const string dateTimeStringFormat = "yyyy-MM-ddTHH:mm:ss";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"/api/1.0/query-with-request-parameter-map-from-supported-types/{stringValue}/{intValue}/{longValue}/{dateTimeValue.ToString(dateTimeStringFormat)}/{boolValue}/{guidValue}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<QueryWithRequestParameterMapFromSupportedTypesScenario.ResponseDto>();

            response.String.Should().Be(stringValue);
            response.Int.Should().Be(intValue);
            response.Long.Should().Be(longValue);
            response.DateTime.ToString(dateTimeStringFormat).Should().Be(dateTimeValue.ToString(dateTimeStringFormat));
            response.Boolean.Should().Be(boolValue);
            response.Guid.Should().Be(guidValue);
        }

        [Fact]
        public async Task RequestParameter()
        {
            var httpClient = CreateClient();

            var input = 2;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"/api/1.0/query-with-request-manual-parameter-with-response/{input}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be("Name - 0 - Request { Id = 2 }");
        }
    }

    [Collection("Sequential")]
    public class Command
    {
        [Fact]
        public async Task Request()
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
        public async Task RequestParameterMapFrom()
        {
            var httpClient = CreateClient();

            var input = 2;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"/api/1.0/command-with-request-parameter-map-from-with-response/{input}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - {input}");
        }

        [Fact]
        public async Task RequestParameterMapFromSupportedTypes()
        {
            var httpClient = CreateClient();

            var stringValue = Guid.NewGuid().ToString();
            var intValue = int.MaxValue;
            var longValue = long.MaxValue;
            var dateTimeValue = DateTime.Now;
            var boolValue = true;
            var guidValue = Guid.NewGuid();

            const string dateTimeStringFormat = "yyyy-MM-ddTHH:mm:ss";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(
                    $"/api/1.0/command-with-request-parameter-map-from-supported-types/{stringValue}/{intValue}/{longValue}/{dateTimeValue.ToString(dateTimeStringFormat)}/{boolValue}/{guidValue}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<CommandWithRequestParameterMapFromSupportedTypesScenario.ResponseDto>();

            response.String.Should().Be(stringValue);
            response.Int.Should().Be(intValue);
            response.Long.Should().Be(longValue);
            response.DateTime.ToString(dateTimeStringFormat).Should().Be(dateTimeValue.ToString(dateTimeStringFormat));
            response.Boolean.Should().Be(boolValue);
            response.Guid.Should().Be(guidValue);
        }

        [Fact]
        public async Task RequestParameter()
        {
            var httpClient = CreateClient();

            var input = 2;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"/api/1.0/command-with-request-manual-parameter-with-response/{input}"),
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - {input}");
        }

        [Fact]
        public async Task RequestPlainText()
        {
            var httpClient = CreateClient();

            var input = Guid.NewGuid().ToString();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/1.0/command-with-request-plain-text-with-response"),
                Content = new StringContent(input, Encoding.UTF8, MediaTypeNames.Text.Plain)
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - 0 - {input}");
        }

        [Fact]
        public async Task RequestUploadFiles()
        {
            var httpClient = CreateClient();

            await using var fileStream = File.OpenRead("./Data/hello-world.txt");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StreamContent(fileStream), name: "hello-world.txt", fileName: "hello-world.txt");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/1.0/command-with-request-upload-files-with-response"),
                Content = multipartFormDataContent
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - 0 - hello-world.txt");
        }

        [Fact]
        public async Task RequestUploadFile()
        {
            var httpClient = CreateClient();

            await using var fileStream = File.OpenRead("./Data/hello-world.txt");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StreamContent(fileStream), name: "hello-world.txt", fileName: "hello-world.txt");
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/1.0/command-with-request-upload-file-with-response"),
                Content = multipartFormDataContent
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - 0 - hello-world.txt");
        }

        [Fact]
        public async Task RequestUploadFileAndJson()
        {
            var httpClient = CreateClient();

            await using var fileStream = File.OpenRead("./Data/hello-world.txt");

            var multipartFormDataContent = new MultipartFormDataContent();
            multipartFormDataContent.Add(new StreamContent(fileStream), name: "hello-world.txt", fileName: "hello-world.txt");

            var id = Guid.NewGuid().ToString();
            var requestDto = new PayloadDto(id);
            multipartFormDataContent.Add(JsonContent.Create(requestDto));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/1.0/command-with-request-upload-file-and-json-with-response"),
                Content = multipartFormDataContent
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<FeatureDto>();

            response.Name.Should().Be($"Name - {id} - 0 - hello-world.txt");
        }

        [Fact]
        public async Task RequestUploadFileAndJsonSupportedTypes()
        {
            var httpClient = CreateClient();

            await using var fileStream = File.OpenRead("./Data/hello-world.txt");

            var multipartFormDataContent = new MultipartFormDataContent();
            var fileName = "hello-world.txt";
            multipartFormDataContent.Add(new StreamContent(fileStream), name: "hello-world.txt", fileName: fileName);

            var stringValue = Guid.NewGuid().ToString();
            var intValue = int.MaxValue;
            var longValue = long.MaxValue;
            var dateTimeValue = DateTime.Now;
            var boolValue = true;
            var guidValue = Guid.NewGuid();

            var requestDto = new CommandWithRequestUploadFileWithPayloadSupportedTypesScenario.CustomPayloadDto(stringValue, intValue, longValue, dateTimeValue, boolValue, guidValue);
            multipartFormDataContent.Add(JsonContent.Create(requestDto));

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/1.0/command-with-request-upload-file-and-json-supported-types"),
                Content = multipartFormDataContent
            };

            var httpResponseMessage = await httpClient.SendAsync(request);

            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<CommandWithRequestUploadFileWithPayloadSupportedTypesScenario.ResponseDto>();

            response.FileName.Should().Be(fileName);
            response.String.Should().Be(stringValue);
            response.Int.Should().Be(intValue);
            response.Long.Should().Be(longValue);
            response.DateTime.Should().Be(dateTimeValue);
            response.Boolean.Should().Be(boolValue);
            response.Guid.Should().Be(guidValue);
        }
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Futurum.WebApiEndpoint.Sample.Program>()
            .CreateClient();
}