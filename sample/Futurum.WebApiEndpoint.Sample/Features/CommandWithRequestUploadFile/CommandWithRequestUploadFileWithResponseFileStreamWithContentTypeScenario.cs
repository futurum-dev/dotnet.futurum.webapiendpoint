using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseFileStreamWithContentTypeScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseFileStream
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }
}