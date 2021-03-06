using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}