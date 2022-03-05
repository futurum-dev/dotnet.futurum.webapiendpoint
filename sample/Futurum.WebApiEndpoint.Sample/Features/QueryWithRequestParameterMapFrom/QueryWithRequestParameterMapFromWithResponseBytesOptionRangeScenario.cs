using System.Text;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseBytesOptionRangeScenario
{
    public record RequestDto
    {
        [MapFromPath("Content")] public string Content { get; set; }
        [MapFromHeader] public Option<Range> Range { get; set; }
    };

    public record Request(string Content, Option<Range> Range);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<RequestDto, Request>.ResponseBytes.Mapper<Mapper>
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            new ResponseBytes(Encoding.UTF8.GetBytes(request.Content))
                {
                    Range = request.Range
                }
                .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.Content, dto.Range).ToResultOkAsync();
    }
}