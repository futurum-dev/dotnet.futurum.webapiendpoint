using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithRequestParameterWithResponseDataCollectionScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponseDataCollection<ApiEndpoint, FeatureDto, Feature>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {query.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Result<Request> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id));
    }
}