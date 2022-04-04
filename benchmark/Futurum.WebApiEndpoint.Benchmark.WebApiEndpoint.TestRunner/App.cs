using System.Net.Mime;
using System.Text;
using System.Text.Json;

using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Benchmark.WebApiEndpoint.TestRunner;

public interface IApp
{
    Task<Result> ExecuteAsync();
}

public class App : IApp
{
    private readonly IHttpClientFactory _httpClientFactory;

    public App(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public Task<Result> ExecuteAsync()
    {
        return Enumerable.Range(0, 1)
                         .FlatMapAsync(async i =>
                         {
                             using (var httpClient = _httpClientFactory.CreateClient())
                             {
                                 return await CallApiEndpoint(httpClient);
                             }
                         });
    }

    private static Task<Result> CallApiEndpoint(HttpClient httpClient)
    {
        async Task<Result> Execute()
        {
            var json = JsonSerializer.Serialize(WebApiEndpointRequest);
            
            var request = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://localhost:5010/api/1.0/benchmark/22"),
                Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json /* or "application/json" in older versions */ ),
            };

            var httpResponseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);
            httpResponseMessage.EnsureSuccessStatusCode();
            
            return httpResponseMessage.IsSuccessStatusCode ? Result.Ok() : Result.Fail($"{httpResponseMessage.ReasonPhrase}");
        }

        return Result.TryAsync(Execute, () => $"");
    }
    
    private static RequestDto WebApiEndpointRequest = new("xxc", "yyy", 23, new[]
    {
        "1111111111",
        "2222222222",
        "3333333333",
        "4444444444",
        "5555555555"
    });
}

public record RequestDto(string? FirstName, string? LastName, int Age, IEnumerable<string>? PhoneNumbers);

public record ResponseDto(int Id, string? Name, int Age, string? PhoneNumber);
