using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<ApiEndpoint, PayloadDto, Payload>.ResponseStream.Mapper<PayloadMapper>
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFileWithPayload<Payload> command, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{command.File.FileName}-{command.Payload.Id}").ToResultOkAsync();
    }
}