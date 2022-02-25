using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponseBytes
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.Files.First().FileName}").ToResultOkAsync();
    }
}