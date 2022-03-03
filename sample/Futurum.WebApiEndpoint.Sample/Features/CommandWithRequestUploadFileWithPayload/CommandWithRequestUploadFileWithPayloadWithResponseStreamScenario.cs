using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFileWithPayload;

public static class CommandWithRequestUploadFileWithPayloadWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFileWithPayload<PayloadDto, Payload>.ResponseStream.Mapper<PayloadMapper>
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFileWithPayload<Payload> request, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{request.File.FileName}-{request.Payload.Id}").ToResultOkAsync();
    }
}