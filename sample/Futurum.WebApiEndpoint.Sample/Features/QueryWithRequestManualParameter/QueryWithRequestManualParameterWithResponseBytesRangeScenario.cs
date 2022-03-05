using System.Text;

using Futurum.Core.Option;
using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseBytesRangeScenario
{
    public record Request(string Content, Range Range);

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
            RangeHeaderMapper.Map(httpContext).ToResult(() => "Unable to get range")
                             .Then(range => httpContext.GetRequestPathParameterAsString("Content")
                                                       .Map(content => new Request(content, range)))
                             .ToResultAsync();
    }
}