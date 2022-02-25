using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.WithResponseEmptyJson.WithMapper<PayloadMapper>
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}