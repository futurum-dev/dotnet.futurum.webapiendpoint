using System.Text.Json.Serialization;

using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.JsonSourceGenerator;

public static class CommandWithRequestWithoutResponseJsonSourceGeneratorScenario
{
    public record CommandDto(string Id);

    public record Command(string Id);

    public record ResponseDto(string Id);

    public record Response(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.Response<ResponseDto, Response>.Mapper<Mapper>
    {
        public override Task<Result<Response>> ExecuteAsync(Command request, CancellationToken cancellationToken) =>
            new Response(request.Id).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>,
                          IWebApiEndpointResponseDtoMapper<Response, ResponseDto>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();

        public ResponseDto Map(Response domain) =>
            new(domain.Id);
    }
}

[JsonSerializable(typeof(CommandWithRequestWithoutResponseJsonSourceGeneratorScenario.CommandDto))]
[JsonSerializable(typeof(CommandWithRequestWithoutResponseJsonSourceGeneratorScenario.ResponseDto))]
public partial class WebApiEndpointJsonSerializerContext : JsonSerializerContext
{
}