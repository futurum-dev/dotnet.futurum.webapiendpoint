using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseBytes
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFile request, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{request.File.FileName}").ToResultOkAsync();
    }
}