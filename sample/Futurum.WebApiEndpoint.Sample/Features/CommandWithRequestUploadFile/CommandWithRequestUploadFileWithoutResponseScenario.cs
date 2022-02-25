using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features.CommandWithRequestUploadFile;

public static class CommandWithRequestUploadFileWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFile<ApiEndpoint>.WithoutResponse
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFile command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}