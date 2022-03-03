using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseBytes
    {
        public override Task<Result<ResponseBytes>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{request.Body}").ToResultOkAsync();
    }
}