using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseStream
    {
        public override Task<Result<ResponseStream>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            new ResponseStream(new FileInfo("./Data/hello-world.txt").OpenRead(), $"hello-world-stream-{request.Body}").ToResultOkAsync();
    }
}