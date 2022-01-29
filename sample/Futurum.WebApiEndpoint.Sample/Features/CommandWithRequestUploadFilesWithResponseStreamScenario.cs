using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithResponseStream
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{command.Files.First().FileName}").ToResultOkAsync();
    }
}