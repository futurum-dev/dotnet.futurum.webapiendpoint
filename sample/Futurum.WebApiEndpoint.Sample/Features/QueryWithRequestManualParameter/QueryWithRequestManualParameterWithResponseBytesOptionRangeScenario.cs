using System.Text;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseBytesOptionRangeScenario
{
    public record Request(string Content, Option<Range> Range);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<Request>.ResponseBytes.Mapper<Mapper>
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            new ResponseBytes(Encoding.UTF8.GetBytes(request.Content))
                {
                    Range = request.Range
                }
                .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Content")
                       .Map(content => new Request(content, RangeHeaderMapper.Map(httpContext)))
                       .ToResultAsync();
    }
}