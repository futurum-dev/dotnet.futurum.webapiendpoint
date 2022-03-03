using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<PayloadDto, Payload>.ResponseEmptyJson.Mapper<PayloadMapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}