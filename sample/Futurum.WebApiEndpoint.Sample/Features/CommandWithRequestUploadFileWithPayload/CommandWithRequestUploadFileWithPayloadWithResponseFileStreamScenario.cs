using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.WithResponseFileStream.WithMapper<PayloadMapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}