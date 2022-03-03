using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.Error;

public static class ErrorExceptionScenario
{
    public record CommandDto(string Id);
    
    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.NoResponse.Mapper<Mapper>
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            throw new Exception($"An exception has occured - {command.Id}");
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }
}