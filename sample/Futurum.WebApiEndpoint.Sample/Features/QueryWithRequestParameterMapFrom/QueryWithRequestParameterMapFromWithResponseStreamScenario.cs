using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseStreamScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<RequestDto, Request>.ResponseStream.Mapper<Mapper>
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead())
                {
                    FileName = $"hello-world-stream-{request.Id}"
                }
                .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.Id).ToResultOkAsync();
    }
}