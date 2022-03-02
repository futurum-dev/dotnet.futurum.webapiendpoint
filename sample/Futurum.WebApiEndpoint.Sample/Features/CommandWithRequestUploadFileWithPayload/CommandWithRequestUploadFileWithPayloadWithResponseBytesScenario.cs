using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.ResponseBytes.Mapper<PayloadMapper>
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.File.FileName}-{command.Payload.Id}").ToResultOkAsync();
    }
}