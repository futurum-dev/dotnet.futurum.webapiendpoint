using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFile<ApiEndpoint>.WithResponseFileStream
    {
        protected override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}