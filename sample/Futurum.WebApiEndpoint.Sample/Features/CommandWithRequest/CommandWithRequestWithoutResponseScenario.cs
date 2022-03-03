using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequest;

public static class CommandWithRequestWithoutResponseScenario
{
    public record CommandDto(string Id);
    
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.NoResponse.Mapper<Mapper>
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            ResponseEmpty.DefaultResultAsync;
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }
}