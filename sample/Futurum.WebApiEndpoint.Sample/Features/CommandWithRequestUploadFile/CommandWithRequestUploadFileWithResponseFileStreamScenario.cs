using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseFileStreamScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.ResponseFileStream
    {
        public override Task<Result<ResponseFileStream>> ExecuteAsync(RequestUploadFile request, CancellationToken cancellationToken) =>
            new ResponseFileStream(new FileInfo("./Data/hello-world.txt")).ToResultOkAsync();
    }
}