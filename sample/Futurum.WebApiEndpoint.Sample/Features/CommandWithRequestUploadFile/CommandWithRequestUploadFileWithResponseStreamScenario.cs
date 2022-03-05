using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseStream
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFile request, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead())
                {
                    FileName = $"hello-world-stream-{request.File.FileName}"
                }
                .ToResultOkAsync();
    }
}