using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class QueryWithRequestParameterMapFromWithResponseScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<RequestDto, Request>.WithResponse<FeatureDto, Feature>
    {
        protected override Task<Result<Feature>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {i} - {query}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Result<Request> Map(HttpContext httpContext, RequestDto dto) =>
            new Request(dto.Id).ToResultOk();
    }
}