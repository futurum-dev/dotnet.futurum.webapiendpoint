using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.NoResponse.Mapper<PayloadMapper>
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}