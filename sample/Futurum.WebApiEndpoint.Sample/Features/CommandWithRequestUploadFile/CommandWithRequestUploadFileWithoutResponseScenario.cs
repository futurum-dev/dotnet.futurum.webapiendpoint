using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.NoResponse
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            ResponseEmpty.DefaultResultAsync;
    }
}