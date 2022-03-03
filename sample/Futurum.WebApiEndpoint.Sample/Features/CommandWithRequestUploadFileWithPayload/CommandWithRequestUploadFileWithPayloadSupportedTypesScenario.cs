using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadSupportedTypesScenario
{
    public record CustomPayloadDto(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record CustomPayload(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record Response(string FileName, string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public record ResponseDto(string FileName, string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<CustomPayloadDto, CustomPayload>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public override Task<Result<Response>> ExecuteAsync(RequestUploadFileWithPayload<CustomPayload> request, CancellationToken cancellationToken) =>
            new Response(request.File.FileName, request.Payload.String, request.Payload.Int, request.Payload.Long, request.Payload.DateTime, request.Payload.Boolean, request.Payload.Guid).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestPayloadMapper<CustomPayloadDto, CustomPayload>,
                          IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Result<CustomPayload> Map(CustomPayloadDto dto) =>
            new CustomPayload(dto.String, dto.Int, dto.Long, dto.DateTime, dto.Boolean, dto.Guid).ToResultOk();

        public ResponseDto Map(Response domain) => 
            new(domain.FileName, domain.String, domain.Int, domain.Long, domain.DateTime, domain.Boolean, domain.Guid);
    }
}