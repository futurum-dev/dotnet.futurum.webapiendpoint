using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseFileStreamWithContentTypeScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/dotnet-logo.png"), "image/png").ToResultOkAsync();
    }
}