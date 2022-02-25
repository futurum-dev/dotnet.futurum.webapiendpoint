using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadSingleFileAndPayloadWithResponseScenario
{
    public record PayloadDto(string Id);

    public record Payload(string Id);

    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.WithResponse<FeatureDto, Feature>.WithMapper<Mapper, FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {command.Payload.Id} - {i} - {command.File.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }

    public class Mapper : IWebApiEndpointRequestPayloadMapper<PayloadDto, Payload>
    {
        public Result<Payload> Map(PayloadDto dto) =>
            new Payload(dto.Id).ToResultOk();
    }
}