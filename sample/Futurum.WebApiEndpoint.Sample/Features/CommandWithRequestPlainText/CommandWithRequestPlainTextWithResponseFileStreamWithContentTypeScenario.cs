using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestPlainText;

public static class CommandWithRequestPlainTextWithResponseFileStreamWithContentTypeScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestPlainText.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestPlainText request, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"))
                {
                    ContentType = "image/png"
                }
                .ToResultOkAsync();
    }
}