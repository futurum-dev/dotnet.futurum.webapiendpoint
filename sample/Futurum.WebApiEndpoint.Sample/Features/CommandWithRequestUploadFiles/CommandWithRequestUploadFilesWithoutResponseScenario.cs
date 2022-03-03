using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles.NoResponse
    {
        public override Task<Result<ResponseEmpty>> ExecuteAsync(RequestUploadFiles request, CancellationToken cancellationToken) =>
            ResponseEmpty.DefaultResultAsync;
    }
}