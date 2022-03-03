using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<PayloadDto, Payload>.Response<FeatureDto, Feature>.Mapper<PayloadMapper, FeatureMapper>
    {
        protected override Task<Result<Feature>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {command.Payload.Id} - {i} - {command.File.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}