using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseFileStreamScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<RequestDto, Request>.ResponseFileStream<ApiEndpoint>.Mapper<Mapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.Id).ToResultOkAsync();
    }
}