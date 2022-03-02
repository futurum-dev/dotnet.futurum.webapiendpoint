using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile<ApiEndpoint>.ResponseStream
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{command.File.FileName}").ToResultOkAsync();
    }
}