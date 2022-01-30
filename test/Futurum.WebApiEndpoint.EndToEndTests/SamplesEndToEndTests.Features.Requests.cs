using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using FluentAssertions;

using Futurum.WebApiEndpoint.Sample.Features;

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
        public async Task RequestParameter()
        {
            var httpClient = CreateClient();
        
            var input = 2;
        
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"/api/1.0/query-with-request-parameter-with-response/{input}"),
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
        public async Task RequestParameter()
        {
            var httpClient = CreateClient();
    
            var input = 2;
    
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"/api/1.0/command-with-request-parameter-with-request/{input}"),
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
    }

    private static HttpClient CreateClient() =>
        new WebApplicationFactory<Futurum.WebApiEndpoint.Sample.Program>()
            .CreateClient();
}