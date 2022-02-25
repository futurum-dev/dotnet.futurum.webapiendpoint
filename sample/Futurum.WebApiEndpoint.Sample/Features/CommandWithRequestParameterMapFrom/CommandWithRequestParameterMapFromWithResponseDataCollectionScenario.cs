using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestParameterMapFrom;

public static class CommandWithRequestParameterMapFromWithResponseDataCollectionScenario
{
    public record CommandDto
    {
        [MapFromPath("Id")] public string Id { get; set; }
    }

    public record Command(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponseDataCollection<ApiEndpoint, FeatureDto, Feature>.WithMapper<Mapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.Id}"))).ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Id).ToResultOk();
    }
}