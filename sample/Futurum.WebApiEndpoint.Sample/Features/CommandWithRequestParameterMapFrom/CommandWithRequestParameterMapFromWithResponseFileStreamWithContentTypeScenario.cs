using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromWithResponseFileStreamWithContentTypeScenario
{
    public record CommandDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseFileStream<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }
}