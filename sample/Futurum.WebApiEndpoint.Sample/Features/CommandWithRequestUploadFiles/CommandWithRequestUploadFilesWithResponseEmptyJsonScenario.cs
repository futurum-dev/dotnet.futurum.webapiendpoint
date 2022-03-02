using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFiles;

public static class CommandWithRequestUploadFilesWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFiles<ApiEndpoint>.ResponseEmptyJson
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}