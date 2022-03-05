using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<PayloadDto, Payload>.ResponseBytes.Mapper<PayloadMapper>
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFileWithPayload<Payload> request, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"))
                {
                    FileName = $"hello-world-bytes-{request.File.FileName}-{request.Payload.Id}"
                }
                .ToResultOkAsync();
    }
}