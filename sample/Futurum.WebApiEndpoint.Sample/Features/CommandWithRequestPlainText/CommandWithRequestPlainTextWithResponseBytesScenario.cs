using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseBytesScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseBytes
    {
        protected override Task<Result<ResponseBytes>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseBytes(File.ReadAllBytes("./Data/hello-world.txt"), $"hello-world-bytes-{command.Body}").ToResultOkAsync();
    }
}