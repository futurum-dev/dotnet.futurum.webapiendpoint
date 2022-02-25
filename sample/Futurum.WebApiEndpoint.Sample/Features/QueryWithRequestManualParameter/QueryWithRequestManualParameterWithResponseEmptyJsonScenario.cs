using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Internal.AspNetCore;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestManualParameter;

public static class QueryWithRequestManualParameterWithResponseEmptyJsonScenario
{
    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<Request>.WithResponseEmptyJson<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<Request>
    {
        public Result<Request> Map(HttpContext httpContext) =>
            httpContext.GetRequestPathParameterAsString("Id")
                       .Map(id => new Request(id));
    }
}