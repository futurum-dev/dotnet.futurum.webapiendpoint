using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithRequestParameterWithResponseScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponse<FeatureDto, Feature>
    {
        protected override Task<Result<Feature>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {query}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Result<Request> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id));
    }
}