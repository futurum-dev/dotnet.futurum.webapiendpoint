using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<Request>.Response<FeatureDto, Feature>.Mapper<Mapper, FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(Request request, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {request}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CancellationToken cancellationToken) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id))
                       .ToResultAsync();
    }
}