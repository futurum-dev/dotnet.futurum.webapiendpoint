using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromSupportedTypesScenario
{
    public record CommandDto
    {
        [MapFromPath("String")] public string String { get; set; }
        [MapFromPath("Int")] public int Int { get; set; }
        [MapFromPath("Long")] public long Long { get; set; }
        [MapFromPath("DateTime")] public DateTime DateTime { get; set; }
        [MapFromPath("Boolean")] public bool Boolean { get; set; }
        [MapFromPath("Guid")] public Guid Guid { get; set; }
    }

    public record Command(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);
    
    public record Response(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);
    
    public record ResponseDto(string String, int Int, long Long, DateTime DateTime, bool Boolean, Guid Guid);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<ResponseDto, Response>.WithMapper<Mapper>
    {
        protected override Task<Result<Response>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new Response(command.String, command.Int, command.Long, command.DateTime, command.Boolean, command.Guid).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>,
                          IWebApiEndpointResponseMapper<Response, ResponseDto>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.String, dto.Int, dto.Long, dto.DateTime, dto.Boolean, dto.Guid).ToResultOk();

        public ResponseDto Map(Response domain) => 
            new(domain.String, domain.Int, domain.Long, domain.DateTime, domain.Boolean, domain.Guid);
    }
}