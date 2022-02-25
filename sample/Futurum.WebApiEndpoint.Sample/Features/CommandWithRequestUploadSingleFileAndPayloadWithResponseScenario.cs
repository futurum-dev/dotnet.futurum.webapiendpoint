using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadSingleFileAndPayloadWithResponseScenario
{
    public record CommandDto
    {
        [MapFromMultipartFile(0)] 
        public IFormFile File { get; set; }

        [MapFromMultipartJson(1)] 
        public PayloadDto Payload { get; set; }
    }

    public record PayloadDto(string Id);

    public record Command(string Id, IFormFile Files);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequest<CommandDto, Command>.WithResponse<FeatureDto, Feature>.WithMapper<Mapper, FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(Command command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {command.Id} - {i} - {command.Files.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestMapper<CommandDto, Command>
    {
        public Result<Command> Map(HttpContext httpContext, CommandDto dto) =>
            new Command(dto.Payload.Id, dto.File).ToResultOk();
    }
}