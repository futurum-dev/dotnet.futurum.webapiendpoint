using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.ResponseBytes
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFiles request, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"))
                {
                    FileName = $"hello-world-bytes-{request.Files.First().FileName}"
                }
                .ToResultOkAsync();
    }
}