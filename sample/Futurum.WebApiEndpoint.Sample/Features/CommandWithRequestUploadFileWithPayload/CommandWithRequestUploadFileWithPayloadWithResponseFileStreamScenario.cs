using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.ResponseFileStream.Mapper<PayloadMapper>
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}