using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithResponseFileStreamWithContentTypeScenario
{
    public record CommandDto(string Id);

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseFileStream<ApiEndpoint>.WithMapper<Mapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }
}