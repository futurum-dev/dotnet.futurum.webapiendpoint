using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Benchmark.Runner;

[
    SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 10000),
    MemoryDiagnoser
]
public class Benchmarks
{
    private static readonly HttpClient WebApiEndpointClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint.Program>().CreateClient();
    private static readonly HttpClient MvcControllerClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.MvcController.Program>().CreateClient();
    private static readonly HttpClient MinimalApiClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.MinimalApi.Program>().CreateClient();

    private static readonly WebApiEndpoint.TestWebApiEndpoint.RequestDto WebApiEndpointRequest = new("FirstName", "LastName", 55, new[]
    {
        "1111111111",
        "2222222222",
        "3333333333",
        "4444444444",
        "5555555555"
    });

    private static readonly MvcController.RequestDto MvcControllerRequest = new("FirstName", "LastName", 55, new[]
    {
        "1111111111",
        "2222222222",
        "3333333333",
        "4444444444",
        "5555555555"
    });

    private static readonly MinimalApi.TestEndpoint.RequestDto MinimalApiRequest = new("FirstName", "LastName", 55, new[]
    {
        "1111111111",
        "2222222222",
        "3333333333",
        "4444444444",
        "5555555555"
    });
    
    [Benchmark]
    public async Task WebApiEndpoints()
    {
        var json = JsonSerializer.Serialize(WebApiEndpointRequest);
            
        var request = new HttpRequestMessage {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/benchmark/22"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await WebApiEndpointClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var body = await httpResponseMessage.Content.ReadFromJsonAsync<WebApiEndpoint.TestWebApiEndpoint.ResponseDto>();
    }

    [Benchmark]
    public async Task MvcControllers()
    {
        var json = JsonSerializer.Serialize(MvcControllerRequest);
            
        var request = new HttpRequestMessage {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/benchmark/22"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await MvcControllerClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var body = await httpResponseMessage.Content.ReadFromJsonAsync<MvcController.ResponseDto>();
    }

    [Benchmark]
    public async Task MinimalApis()
    {
        var json = JsonSerializer.Serialize(MinimalApiRequest);
            
        var request = new HttpRequestMessage {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/benchmark/22"),
            Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)
        };

        var httpResponseMessage = await MinimalApiClient.SendAsync(request);

        httpResponseMessage.EnsureSuccessStatusCode();
        var body = await httpResponseMessage.Content.ReadFromJsonAsync<MinimalApi.TestEndpoint.ResponseDto>();
    }
}