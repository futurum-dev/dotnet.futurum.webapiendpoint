using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromWithResponseDataCollectionScenario
{
    public record RequestDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Request(string Id);

    public class ApiEndpoint : QueryWebApiEndpoint.Request<RequestDto, Request>.ResponseDataCollection<FeatureDto, Feature>.Mapper<Mapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(Request query, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {query.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<RequestDto, Request>
    {
        public Task<Result<Request>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, RequestDto dto, CancellationToken cancellationToken) =>
            new Request(dto.Id).ToResultOkAsync();
    }
}