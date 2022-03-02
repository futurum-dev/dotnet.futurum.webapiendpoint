using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseDataCollectionScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.ResponseDataCollection<FeatureDto, Feature>.Mapper<PayloadMapper, FeatureDataMapper>
    {
        protected override Task<Result<ResponseDataCollection<Feature>>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseDataCollection<Feature>(Enumerable.Range(0, 10).Select(i => new Feature($"Name - {i} - {command.File.FileName} - {command.Payload.Id}"))).ToResultOkAsync();
    }
}