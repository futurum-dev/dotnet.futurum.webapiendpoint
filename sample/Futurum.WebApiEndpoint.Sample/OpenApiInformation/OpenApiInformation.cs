using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.OpenApiInformation;

public static class OpenApiInformation
{
    public record Response(int Number);

    public record ResponseDto(int Number);

    public class ApiEndpoint : QueryWebApiEndpoint.WithoutRequest.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(CancellationToken cancellationToken) =>
            new Response(10).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public ResponseDto Map(Response domain) =>
            new (domain.Number);
    }
}