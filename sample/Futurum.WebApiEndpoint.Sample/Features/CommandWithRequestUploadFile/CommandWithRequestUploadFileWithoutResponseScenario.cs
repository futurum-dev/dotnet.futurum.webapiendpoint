using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile.NoResponse
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(RequestUploadFile request, CancellationToken cancellationToken) =>
            ResponseEmpty.DefaultResultAsync;
    }
}