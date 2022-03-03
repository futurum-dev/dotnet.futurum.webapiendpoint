using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseFileStream
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(RequestPlainText command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}