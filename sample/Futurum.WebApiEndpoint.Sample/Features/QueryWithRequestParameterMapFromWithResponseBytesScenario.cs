using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithRequestParameterMapFromWithResponseBytesScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponseBytes<ApiEndpoint>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{query.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request(dto.Id).ToResultOk();
    }
}