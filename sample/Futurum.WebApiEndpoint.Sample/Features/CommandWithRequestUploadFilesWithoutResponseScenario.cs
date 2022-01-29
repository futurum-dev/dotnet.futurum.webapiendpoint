using Futurum.Core.Result;

namespace Futurum.WebApiEndpoint.Sample.Features;

public static class CommandWithRequestUploadFilesWithoutResponseScenario
{
    public class ApiEndpoint : CommandWebApiEndpoint.WithRequestUploadFiles<ApiEndpoint>.WithoutResponse
    {
        protected override Task<Result> ExecuteAsync(RequestUploadFiles command, CancellationToken cancellationToken) =>
            Result.OkAsync();
    }
}