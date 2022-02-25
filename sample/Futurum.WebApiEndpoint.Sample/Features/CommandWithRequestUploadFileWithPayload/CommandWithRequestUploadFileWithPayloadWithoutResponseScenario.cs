using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.WithoutResponse.WithMapper<PayloadMapper>
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}