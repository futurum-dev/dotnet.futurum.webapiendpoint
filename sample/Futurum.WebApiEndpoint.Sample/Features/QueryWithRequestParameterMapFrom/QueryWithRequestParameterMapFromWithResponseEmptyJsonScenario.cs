using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseEmptyJsonScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponseEmptyJson<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request(dto.Id).ToResultOk();
    }
}