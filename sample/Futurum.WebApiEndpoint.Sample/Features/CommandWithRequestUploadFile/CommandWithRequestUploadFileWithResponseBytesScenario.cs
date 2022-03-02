using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile<ApiEndpoint>.ResponseBytes
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.File.FileName}").ToResultOkAsync();
    }
}