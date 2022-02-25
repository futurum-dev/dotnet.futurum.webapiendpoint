using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithResponseEmptyJsonScenario
{
    public record CommandDto(string Id);

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseEmptyJson<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }
}