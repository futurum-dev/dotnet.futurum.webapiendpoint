using System.Net.Mime;
using System.Text;
using System.Text.Json;

using BenchmarkDotNet.Attributes;

using Microsoft.AspNetCore.Mvc.Testing;

namespace Futurum.WebApiEndpoint.Benchmark.Runner;

[
    MemoryDiagnoser,
    SimpleJob(launchCount: 1, warmupCount: 5, targetCount: 20, invocationCount: 20000)
]
public class Benchmarks
{
    private static readonly HttpClient WebApiEndpointClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint.Program>().CreateClient();
    private static readonly HttpClient MvcControllerClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.MvcController.Program>().CreateClient();
    private static readonly HttpClient MinimalApiClient = new WebApplicationFactory<Futurum.WebApiEndpoint.Benchmark.MinimalApi.Program>().CreateClient();

    private static readonly StringContent WebApiEndpointRequestPayload =
        new(JsonSerializer.Serialize(new WebApiEndpoint.TestWebApiEndpoint.RequestDto("FirstName", "LastName", 55, new[]
                                         {
                                             "1111111111",
                                             "2222222222",
                                             "3333333333",
                                             "4444444444",
                                             "5555555555"
                                         }
                                     )),
            Encoding.UTF8, MediaTypeNames.Application.Json);

    private static readonly StringContent MvcControllerRequestPayload =
        new(JsonSerializer.Serialize(new MvcController.RequestDto("FirstName", "LastName", 55, new[]
            {
                "1111111111",
                "2222222222",
                "3333333333",
                "4444444444",
                "5555555555"
            })),
            Encoding.UTF8, MediaTypeNames.Application.Json);

    private static readonly StringContent MinimalApiRequestPayload =
        new(JsonSerializer.Serialize(new MinimalApi.TestEndpoint.RequestDto("FirstName", "LastName", 55, new[]
            {
                "1111111111",
                "2222222222",
                "3333333333",
                "4444444444",
                "5555555555"
            })),
            Encoding.UTF8, MediaTypeNames.Application.Json);

    [Benchmark(Baseline = true)]
    public Task WebApiEndpoints()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/1.0/benchmark/22"),
            Content = WebApiEndpointRequestPayload
        };

        return WebApiEndpointClient.SendAsync(request);
    }

    [Benchmark]
    public Task MvcControllers()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/benchmark/22"),
            Content = MvcControllerRequestPayload
        };

        return MvcControllerClient.SendAsync(request);
    }

    [Benchmark]
    public Task MinimalApis()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("/api/benchmark/22"),
            Content = MinimalApiRequestPayload
        };

        return MinimalApiClient.SendAsync(request);
    }
}