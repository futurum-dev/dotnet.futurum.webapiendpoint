using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseAsyncEnumerableScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<Request>.ResponseAsyncEnumerable<FeatureDto, Feature>.Mapper<Mapper, FeatureDataMapper>
    {
        public override Task<Result<ResponseAsyncEnumerable<Feature>>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            new ResponseAsyncEnumerable<Feature>(AsyncEnumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {request.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id))
                       .ToResultAsync();
    }
}