using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<PayloadDto, Payload>.Response<FeatureDto, Feature>.Mapper<PayloadMapper, FeatureMapper>
    {
        public override Task<Result<Feature>> ExecuteAsync(RequestUploadFileWithPayload<Payload> request, CancellationToken cancellationToken) =>
            Enumerable.Range(0, 10)
                      .Select(i => new Feature($"Name - {request.Payload.Id} - {i} - {request.File.FileName}"))
                      .First()
                      .ToResultOkAsync();
    }
}