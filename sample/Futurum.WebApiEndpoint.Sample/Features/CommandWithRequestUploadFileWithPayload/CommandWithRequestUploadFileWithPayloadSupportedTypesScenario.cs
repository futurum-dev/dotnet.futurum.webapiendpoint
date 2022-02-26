using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadSupportedTypesScenario
{
    public record CustomPayloadDto(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record CustomPayload(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record Response(string FileName, string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record ResponseDto(string FileName, string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, CustomPayloadDto, CustomPayload>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(RequestUploadFileWithPayload<CustomPayload> command, CancellationToken cancellationToken) =>
            new Response(command.File.FileName, command.Payload.String, command.Payload.Int, command.Payload.Long, command.Payload.DateTime, command.Payload.Boolean, command.Payload.Guid).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestPayloadMapper<CustomPayloadDto, CustomPayload>,
                          IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<CustomPayload> Map(CustomPayloadDto dto) =>
            new CustomPayload(dto.String, dto.Int, dto.Long, dto.DateTime, dto.Boolean, dto.Guid).ToResultOk();

        public ResponseDto Map(HttpContext httpContext, Response domain) => 
            new(domain.FileName, domain.String, domain.Int, domain.Long, domain.DateTime, domain.Boolean, domain.Guid);
    }
}