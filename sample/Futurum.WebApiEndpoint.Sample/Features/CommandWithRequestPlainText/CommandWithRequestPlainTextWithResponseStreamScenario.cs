using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText<ApiEndpoint>.ResponseStream
    {
        protected override Task<Result<ResponseStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{command.Body}").ToResultOkAsync();
    }
}