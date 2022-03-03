using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseBytesScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<RequestDto, Request>.ResponseBytes.Mapper<Mapper>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{query.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.Id).ToResultOkAsync();
    }
}