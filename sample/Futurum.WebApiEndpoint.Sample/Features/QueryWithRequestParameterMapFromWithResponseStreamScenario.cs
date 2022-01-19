using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithRequestParameterMapFromWithResponseStreamScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponseStream<ApiEndpoint>
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{query.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request(dto.Id).ToResultOk();
    }
}