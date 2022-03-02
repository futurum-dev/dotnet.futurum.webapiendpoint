using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithResponseEmptyJsonScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.RequestUploadFile<ApiEndpoint>.ResponseEmptyJson
    {
        protected override Task<Result<ResponseEmptyJson>> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            new ResponseEmptyJson().ToResultOkAsync();
    }
}