using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromWithResponseStreamScenario
{
    public record CommandDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseStream<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{command.Id}").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }
}