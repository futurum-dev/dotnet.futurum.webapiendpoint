using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.QueryWithRequestParameterMapFrom;

public static class QueryWithRequestParameterMapFromSupportedTypesScenario
{
    public record QueryDto
    {
        [MapFromPath("String")] public string String { get; set; }
        [MapFromPath("Int")] public int Int { get; set; }
        [MapFromPath("Long")] public long Long { get; set; }
        [MapFromPath("DateTime")] public DateTime DateTime { get; set; }
        [MapFromPath("Boolean")] public bool Boolean { get; set; }
        [MapFromPath("Guid")] public Guid Guid { get; set; }
    }

    public record Query(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);
    
    public record Response(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);
    
    public record ResponseDto(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public class ApiEndpoint : QueryWebApiEndpoint.WithRequest<QueryDto, Query>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Query query, CancellationToken cancellationToken) =>
            new Response(query.String, query.Int, query.Long, query.DateTime, query.Boolean, query.Guid).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<QueryDto, Query>,
                          IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Query> Map(HttpContext httpContext, QueryDto dto) =>
            new Query(dto.String, dto.Int, dto.Long, dto.DateTime, dto.Boolean, dto.Guid).ToResultOk();

        public ResponseDto Map(HttpContext httpContext, Response domain) => 
            new(domain.String, domain.Int, domain.Long, domain.DateTime, domain.Boolean, domain.Guid);
    }
}