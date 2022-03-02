using Futurum.Core.Result;
using Futurum.WebApiEndpoint.Metadata;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromWithResponseEmptyJsonScenario
{
    public record CommandDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.Request<CommandDto, Command>.ResponseEmptyJson<ApiEndpoint>.Mapper<Mapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Task<Result<Command>> MapAsync(HttpContext httpContext, MetadataDefinition metadataDefinition, CommandDto dto, CancellationToken cancellationToken) =>
            new Command(dto.Id).ToResultOkAsync();
    }
}