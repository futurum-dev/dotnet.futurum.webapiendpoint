using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromWithResponseBytesScenario
{
    public record CommandDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseBytes<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }
}